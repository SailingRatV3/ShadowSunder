using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StormMageBoss : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 1;
    public float knockbackForce = 300f;
    public float movementSpeed = 500f;
    public UIManager uiManager;
    
    [Header("Boss Fight Settings")]
    public DetectionZone detectionZone;
    public float lightningStrikeHealthThresholf = 2f;
    public int hitsBeforeRadialAttack = 3;
    public float restDuration = 3f;
    public Transform[] movePositions;
    public Transform restPositions;
    
    Rigidbody2D rb;
    DamageableCharacters damageableCharacter;
    RadialAttack shootRadial;
    BossLightningStrike bossLightningStrike;
    PlayerController player;

    private int timesHit = 0;    // Number of times hit
    private float restTimer = 0f; // Resting timer
    
    private BossPhase currentPhase = BossPhase.Inactive; // Boss is Inactive
    
    private Vector2 currentTargetPosition;
    private bool isMoving = false;
    private bool isAttacking = false;
    
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
        damageableCharacter = GetComponent<DamageableCharacters>();
        shootRadial = GetComponent<RadialAttack>();
        bossLightningStrike = GetComponent<BossLightningStrike>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        damageableCharacter.OnDamage += HandleBossDamaged;
        damageableCharacter.OnDeath += HandleBossDeath;
    }

    void Update()
    {    
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
                StartCoroutine(DoRadialAttack());
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

    public void StartBossPhase()
    {
        currentPhase = BossPhase.Phase1;
    }

    void HandlePhase1()
    {
        if (player == null) return;
        
        if (!isMoving)
        {
            // Random target position to move
            currentTargetPosition = movePositions[Random.Range(0, movePositions.Length)].position;
            isMoving = true;
        }

        if (isMoving)
        {
            Vector2 direction = (currentTargetPosition - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * movementSpeed;

            if (Vector2.Distance(transform.position, currentTargetPosition) < 0.5f)
            {
                rb.linearVelocity = Vector2.zero;
                isMoving = false;
                StartCoroutine(ShootAtPlayer());
            }
        }
        // Check Health
        if (damageableCharacter.Health <= lightningStrikeHealthThresholf)
        {
            currentPhase = BossPhase.LightningStrike;
        }
    }

    IEnumerator ShootAtPlayer()
    {
        isAttacking = true;

        if (shootRadial != null)
        {
            yield return shootRadial.ShootRadialBursts();
        }
        isAttacking = false;
    }

    void HandleLightningPhase()
    {
        rb.linearVelocity = Vector2.zero;
        if (!isAttacking)
        {
            StartCoroutine(DoLightningPhase());
        }
    }

    IEnumerator DoLightningPhase()
    {
        isAttacking = true;
        bossLightningStrike.TriggerLightningStrike(player.transform);
        yield return new WaitForSeconds(2f); // Lightning Animation
        restTimer = restDuration;
        currentPhase = BossPhase.Resting;
        isAttacking = false;
    }

    IEnumerator DoRadialAttack()
    {
        isAttacking = true;
        if (shootRadial != null)
        {
            yield return shootRadial.ShootRadialBursts();
        }
        isAttacking = false;
        currentPhase = BossPhase.Phase1;
        yield return null;
    }

    void HandleBossDamaged()
    {
        timesHit++;
        if (timesHit >= hitsBeforeRadialAttack)
        {
            currentPhase = BossPhase.RadialAttack;
            timesHit = 0;
        }
    }

    void HandleBossDeath()
    {
        currentPhase = BossPhase.Dead;
        rb.linearVelocity = Vector2.zero;
        shootRadial.isShooting = false;
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
