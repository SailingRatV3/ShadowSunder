using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StormMageBoss : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 1;
    public float knockbackForce = 300f;
    public float movementSpeed = 2f; // Boss Movement
    public UIManager uiManager;
    
    [Header("Boss Fight Settings")]
    public DetectionZone detectionZone;
   // public float lightningStrikeHealthThresholf = 2f;
    [Range(0f, 1f)]
    public float lightningStrikeHealthThreshold = 0.5f;
    public int hitsBeforeRadialAttack = 3;
    public float restDuration = 3f;
    public Transform[] movePositions;
    public Transform restPositions;
    
    Rigidbody2D rb;
    DamageableCharacters damageableCharacter;
    CharacterHealth healthCharacter;
    RadialAttack shootRadial;
    BossLightningStrike bossLightningStrike;
    PlayerController player;

    private int timesHit = 0;    // Number of times hit
    private float restTimer = 0f; // Resting timer
    
    private BossPhase currentPhase = BossPhase.Inactive; // Boss is Inactive
    
    private Vector2 currentTargetPosition;
    private bool isMoving = false;
    private bool isAttacking = false;
    private bool isRadialAttacking = false;
    private bool pendingRadialAttack = false;
    private bool isWaitingAfterMove = false;
    private bool isDead = false;
    private enum BossPhase
    {
        Inactive,
        Phase1,
        LightningStrike,
        RadialAttack,
        Resting,
        Dead
    } 
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
       // rb.isKinematic = true;  // manually move
       // rb.simulated = false;   // Preventing physics interference
        healthCharacter = GetComponent<CharacterHealth>();
        damageableCharacter = GetComponent<DamageableCharacters>();
        shootRadial = GetComponent<RadialAttack>();
        bossLightningStrike = GetComponent<BossLightningStrike>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        damageableCharacter.OnDamage += HandleBossDamaged;
        damageableCharacter.OnDeath += HandleBossDeath;
    }

    void Update()
    {
        //Debug.Log($"Phase: {currentPhase}, IsDead: {isDead}");
        if (!isDead && damageableCharacter._health <= 0)
        {
           // Debug.Log("On Death not called");
            HandleBossDeath();
        }
        
        if (currentPhase == BossPhase.Dead) return;
        // Phase Switch
        switch (currentPhase)
        {
            case BossPhase.Inactive:
                // Wait for Boss Trigger
                break;
            case BossPhase.Phase1:
                HandlePhase1();
                break;
            case BossPhase.LightningStrike:
                HandleLightningPhase();
                break;

            case BossPhase.RadialAttack:
                if (!isRadialAttacking && !isDead)
                { 
                    StartCoroutine(DoRadialAttack());
                }
                break;
            case BossPhase.Resting:
                restTimer -= Time.deltaTime;
                if (restTimer <= 0f)
                {
                    currentPhase = BossPhase.Phase1;
                }
                break;
            case BossPhase.Dead:
                // No Actions 
                // Add Ending Cutscene
                break;
        }
        
        
        // Collider2D detectedObject0 = detectionZone.detectedObjects[0];

        if (damageableCharacter.Targetable && detectionZone.detectedObjects.Count > 0)
        {
            uiManager.moveBossPanel();
            // radial Projectile
            /*
            shootRadial.isShooting = true;
            StartCoroutine(shootRadial.ShootRadialBursts());
            */
            
            // testing lighningstrike
           // bossLightningStrike.TriggerLightningStrike(player.transform);
           // StartCoroutine(bossLightningStrike.LightningStrikeCoroutine(player.transform));
            
            
            // Calc direction to target 0 
            //Vector2 direction = (detectionZone.detectedObjects[0].transform.position - transform.position).normalized;
            
            // move to first detected object
           // rb.AddForce(direction * (movementSpeed * Time.deltaTime));
        }
        else
        {
            // shootRadial.isShooting = false;
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentTargetPosition, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    public void StartBossPhase()
    {
        currentPhase = BossPhase.Phase1;
    }

    void HandlePhase1()
    {
       // Debug.Log("HandlePhase1");
        
        if (player == null) return;
        
        if (!isMoving && !isAttacking && !isWaitingAfterMove)
        {
           // Debug.Log("Moving to new Position");
            // Random target position to move
            currentTargetPosition = movePositions[Random.Range(0, movePositions.Length)].position;
            isMoving = true;
        }

        if (isMoving)
        {
          //  Debug.Log("Is Moving");
            Vector2 newPos = Vector2.MoveTowards(rb.position, currentTargetPosition, movementSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
            //Vector2 direction = (currentTargetPosition - (Vector2)transform.position).normalized;
         //   rb.linearVelocity = direction * movementSpeed;
         //rb.AddForce(direction * (movementSpeed * Time.deltaTime));
        // Debug.Log($"Target: {currentTargetPosition}, Current: {rb.position}");
            if (Vector2.Distance(rb.position, currentTargetPosition) < 0.5f)
            {
                isMoving = false;

                if (pendingRadialAttack)
                {
                    pendingRadialAttack = false;
                    currentPhase = BossPhase.RadialAttack;
                    //yield break;
                }
                
                StartCoroutine(WaitAndAttack());
            }
        }

        float healthPercent = damageableCharacter._health / damageableCharacter._maxHealth;  
        // Check Health
        if (!isDead && currentPhase != BossPhase.LightningStrike && healthPercent<= lightningStrikeHealthThreshold)
        {
            currentPhase = BossPhase.LightningStrike;
        }
    }

    IEnumerator WaitAndAttack()
    {
        isWaitingAfterMove = true;
        yield return new WaitForSeconds(0.5f); // pause
        yield return ShootAtPlayer(); // shoot radial
        
        restTimer = restDuration;
        currentPhase = BossPhase.Resting;
        isWaitingAfterMove = false;
    }

    IEnumerator ShootAtPlayer()
    {
        Debug.Log("Shoot Radial");
        isAttacking = true;

        if (shootRadial != null)
        {
            shootRadial.isShooting = true;
            yield return StartCoroutine(shootRadial.ShootRadialBursts());
            shootRadial.isShooting = false;
        }
        isAttacking = false;
       // currentPhase = BossPhase.Phase1;
        // yield return null;
    }

    void HandleLightningPhase()
    {
        if (isDead) return;
        Debug.Log("Handling Lightning Phase");
      //  rb.linearVelocity = Vector2.zero;
        if (!isAttacking)
        {
            StartCoroutine(DoLightningPhase());
        }
    }

    IEnumerator DoLightningPhase()
    {
        Debug.Log("Doing Lightning Phase");
        if(isDead) yield break;
        
        isAttacking = true;
        bossLightningStrike.TriggerLightningStrike(player.transform);
        yield return new WaitForSeconds(2f); // Lightning Animation
        
        if(isDead) yield break;
        
        restTimer = restDuration;
        currentPhase = BossPhase.Phase1;
        isAttacking = false;
    }

    IEnumerator DoRadialAttack()
    {
        Debug.Log("Doing Radial Attack");
        isAttacking = true;
        isRadialAttacking = true;
        if (shootRadial != null)
        {
            Debug.Log("Shoot Radial");
            shootRadial.isShooting = true;
            StartCoroutine(shootRadial.ShootRadialBursts());
            shootRadial.isShooting = false;
        }
       

        isAttacking = false;
        isRadialAttacking = false;
        currentPhase = BossPhase.Phase1;
         yield return null;
    }

    void HandleBossDamaged()
    {
        Debug.Log("Handle Boss Damaged");
        timesHit++;
        if (timesHit >= hitsBeforeRadialAttack)
        {
            
            timesHit = 0;
            if (isMoving)
            {
                pendingRadialAttack = true;
            }
            else
            {
                currentPhase = BossPhase.RadialAttack;

            }
        }
    }

    void HandleBossDeath()
    {
        isDead = true;
        Debug.Log("Handle Boss Death");
        currentPhase = BossPhase.Dead;
        //rb.linearVelocity = Vector2.zero;
        shootRadial.isShooting = false;
        StopAllCoroutines();
        // Defeated Animation
        // Ending Cutscene
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        
        Collider2D collider = col.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            
            // Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;
            Vector3 parentPosition = transform.position; // Get sprite orgin position
            Vector2 direction = (Vector2) (collider.gameObject.transform.position - transform.position).normalized; // normalized to not change the magnitude
            Vector2 knockback = direction * knockbackForce;
                  
            damageable.OnHit(damage, knockback); // implement OnHit

        }
    }
    
}
