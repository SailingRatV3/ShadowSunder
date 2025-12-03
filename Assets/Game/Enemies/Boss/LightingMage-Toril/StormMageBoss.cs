using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class StormMageBoss : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 1;
    public float knockbackForce = 300f;
    public float movementSpeed = 2f; // Boss Movement
    public UIManager uiManager;
    public ShadowDive shadowDive;
    
    [Header("Boss Fight Settings")]
    // First Phase
    public DetectionZone detectionZone;
   
    [Range(0f, 1f)]
    public float lightningStrikeHealthThreshold = 0.5f;
    public int hitsBeforeRadialAttack = 3;
    public float restDuration = 3f;
    public Transform[] movePositions;
    // Second Phase
    public Transform centerPosition;
    public GameObject barrierObject; 
    public List<Transform> lightSpawnPoints; 
    public GameObject lightPrefab;
    public float pushbackForce = 500f;
   
    [Header("Phase 2 Teleport Settings")]
    public Transform[] teleportPositions;
    public float teleportDelay = 0.2f;
    private float lastTeleportTime = -999f;
    public float teleportCooldown = 2f;
    
    private List<GameObject> spawnedLights = new List<GameObject>();
    private bool barrierBroken = false;
    private bool hasSpawnedLights = false;
    
    [Header("Knockback Settings")]
    private Dictionary<Collider2D, float> knockbackCooldowns = new Dictionary<Collider2D, float>();
    public float knockbackCooldown = 0.5f;
    public float knockbackMultiplier = 0.5f;
    
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
    private bool isReturningToCenter = false;
    private enum BossPhase
    {
        Inactive,
        Phase1,
        LightningStrike,
        BarrierBreakSequence01,
        LightningSetup,
        WaitingLightHit,
        RadialAttack,
        Phase2,
        Resting,
        Dead
    } 
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
       
        healthCharacter = GetComponent<CharacterHealth>();
        damageableCharacter = GetComponent<DamageableCharacters>();
        shootRadial = GetComponent<RadialAttack>();
        bossLightningStrike = GetComponent<BossLightningStrike>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        shadowDive = GameObject.FindGameObjectWithTag("Player").GetComponent<ShadowDive>();
        
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
            case BossPhase.Phase2:
                HandlePhase2();
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
           
            case BossPhase.LightningSetup:
                StartCoroutine(SetupLightningPhase());
                break;
            case BossPhase.BarrierBreakSequence01:
                StartCoroutine(HandleBarrierBreakSequence());
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
        
        
        

        if (damageableCharacter.Targetable && detectionZone.detectedObjects.Count > 0)
        {
            uiManager.moveBossPanel();
            
        }
        
        
        
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentTargetPosition, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
    
    IEnumerator ReturnToCenter()
    {
        isReturningToCenter = true;

        float distance = Vector2.Distance(transform.position, centerPosition.position);
        while (distance > 0.05f)
        {
            Vector2 direction = ((Vector2)centerPosition.position - rb.position).normalized;
            rb.linearVelocity = direction * movementSpeed;

            yield return null;

            distance = Vector2.Distance(transform.position, centerPosition.position);
        }

        

        rb.linearVelocity = Vector2.zero; 
        isReturningToCenter = false;
    }

    public void StartBossPhase()
    {
        currentPhase = BossPhase.Phase1;
    }

    void HandlePhase1()
    {
       if (player == null) return;
        
        if (!isMoving && !isAttacking && !isWaitingAfterMove)
        {
           
            // Random target position to move
            currentTargetPosition = movePositions[Random.Range(0, movePositions.Length)].position;
            isMoving = true;
        }

        if (isMoving)
        {
          //  Debug.Log("Is Moving");
            Vector2 newPos = Vector2.MoveTowards(rb.position, currentTargetPosition, movementSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
            
            
            
            
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
            currentPhase = BossPhase.LightningSetup;
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
            if (currentPhase == BossPhase.Phase2)
            {
                // If want the projectile speed to gradually increase
               // shootRadial.projectileSpeed += 3f;
               shootRadial.projectileSpeed = 8f;
               // Debug.Log("Projectile Speed: " + shootRadial.projectileSpeed);
            }
            yield return StartCoroutine(shootRadial.ShootRadialBursts());
            shootRadial.isShooting = false;
        }
        isAttacking = false;
       
    }

    void HandleLightningPhase()
    {
        if (isDead) return;
        //Debug.Log("Handling Lightning Phase");
      
        if (!isAttacking)
        {
            StartCoroutine(DoLightningPhase());
        }
    }

    IEnumerator DoLightningPhase()
    {
        // Debug.Log("Doing Lightning Phase");
        if(isDead) yield break;
        
        isAttacking = true;
        bossLightningStrike.TriggerLightningStrike(player.transform);
        
        yield return new WaitForSeconds(2f); // Lightning Animation
        
        if(isDead) yield break;
        
        restTimer = restDuration;
        currentPhase = BossPhase.LightningStrike;
        isAttacking = false;
    }

    IEnumerator DoRadialAttack()
    {
       // Debug.Log("Doing Radial Attack");
        isAttacking = true;
        isRadialAttacking = true;
        if (shootRadial != null)
        {
           // Debug.Log("Shoot Radial");
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
        timesHit++;
        Debug.Log($"Boss Damaged | Barrier Active: {barrierObject.activeSelf} | IsRadialAttacking: {isRadialAttacking} | IsAttacking: {isAttacking} | IsDead: {isDead}");
        
        if (barrierObject.activeSelf && currentPhase == BossPhase.Phase2 && !isRadialAttacking && !isDead)
        {
           // Debug.Log("Boss hit during Phase 2");
            StartCoroutine(TeleportBossAndBarrier());
            return;
            
            /*
             * if (Time.time - lastTeleportTime >= teleportCooldown)
    {
        lastTeleportTime = Time.time;
        StartCoroutine(TeleportBossAndBarrier());
    }
    return;
             */
            
        }

        
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
    IEnumerator ReactWithRadialAttack()
    {
        Debug.Log("Starting ReactWithRadialAttack");
        isRadialAttacking = true;
        isAttacking = true;

        if (shootRadial != null)
        {
            Debug.Log("Calling ShootRadialBursts");
            shootRadial.isShooting = true;
            yield return StartCoroutine(shootRadial.ShootRadialBursts());
            shootRadial.isShooting = false;
        }
        else
        {
            Debug.LogWarning("shootRadial is null!");
        }

        Debug.Log("Finishing ReactWithRadialAttack");

        isAttacking = false;
        isRadialAttacking = false;
    }

    void HandleBossDeath()
    {
        isDead = true;
        currentPhase = BossPhase.Dead;
    
        shootRadial.isShooting = false;
        StopAllCoroutines();
    
        damageableCharacter.OnDamage -= HandleBossDamaged;

        Debug.Log("Handle Boss Death");
        // Defeated Animation
        // Ending Cutscene
    }

    void HandlePhase2()
    {
        currentPhase = BossPhase.Phase2;
       
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        
        Collider2D collider = col.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            
            Vector3 parentPosition = transform.position; 
            Vector2 direction = (Vector2) (collider.gameObject.transform.position - transform.position).normalized; // normalized to not change the magnitude
           
            Vector2 knockback = direction * knockbackForce;
                  
            damageable.OnHit(damage, knockback); 

           
            
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        Collider2D collider = col.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if (damageable != null)
        {
            if (!knockbackCooldowns.ContainsKey(collider))
            {
                knockbackCooldowns[collider] = 0f;
            }

            if (Time.time >= knockbackCooldowns[collider])
            {
                Vector2 knockbackDir;

                if (rb.linearVelocity.magnitude > 0.1f)
                {
                    knockbackDir = rb.linearVelocity.normalized; 
                }
                else
                {
                    knockbackDir = (collider.transform.position - transform.position).normalized;
                }

                Vector2 knockback = knockbackDir * knockbackForce;
                damageable.OnHit(damage, knockback);

                knockbackCooldowns[collider] = Time.time + knockbackCooldown;
            }
        }
    }
    
    IEnumerator SetupLightningPhase()
    {
        currentPhase = BossPhase.Inactive; 
        isAttacking = true;
        
        // Push Player
        if (player != null  && !hasSpawnedLights) 
        {
           
        }
        
        // Move to center
        while (Vector2.Distance(transform.position, centerPosition.position) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, centerPosition.position, movementSpeed * Time.deltaTime));
            yield return null;
        }

        damageableCharacter.canRecieveKnockback = false;

        // Spawn Lights
        if (!hasSpawnedLights)
        {
            foreach (var spawnPoint in lightSpawnPoints)
            {
                GameObject light = Instantiate(lightPrefab, spawnPoint.position, Quaternion.identity);
                spawnedLights.Add(light);
            }

            shadowDive.RefreshLightList();
            hasSpawnedLights = true;
        }
        

        // Activate Boss Barrier
        barrierObject.SetActive(true);
       // Debug.Log("Barrier enabled: " + barrierObject.activeSelf);
        damageableCharacter.Targetable = true;

        yield return new WaitForSeconds(1.5f);
        currentPhase = BossPhase.Phase2;


        damageableCharacter.OnDamage += HandleBossDamaged;
       // Debug.Log("Subscribed HandleBossDamaged to OnDamage event");

        StartCoroutine(Phase2ProjectileRoutine());
        isAttacking = false;
        
       
    }
    
    
    
    public void OnBarrierBrokenByLight()
    {
        if (currentPhase != BossPhase.WaitingLightHit) return;

        barrierBroken = true;
        currentPhase = BossPhase.BarrierBreakSequence01;
    }
    
    /*
     * Enumerators --------------------------------------------------------------------------------------------------------------------------------------------------
     */
    
    IEnumerator HandleBarrierBreakSequence()
    {
        isAttacking = true;

        // Disable barrier
        barrierObject.SetActive(false);
        damageableCharacter.Targetable = true;

        yield return new WaitForSeconds(0.5f);

        // If player is in the center, push them back

        yield return new WaitForSeconds(1f);

        // Pull lights to boss or reset them
        foreach (var light in spawnedLights)
        {
            if (light != null)
            {
                
                StartCoroutine(MoveObjectToTarget(light.transform, transform.position));
            }
        }

        yield return new WaitForSeconds(2f);

        // Reactivate barrier
        barrierObject.SetActive(true);
        Debug.Log("Barrier enabled: " + barrierObject.activeSelf);
        damageableCharacter.Targetable = false;

        // Begin lightning strike attacks
        currentPhase = BossPhase.LightningStrike;
        isAttacking = false;
    }
    
    IEnumerator MoveObjectToTarget(Transform obj, Vector3 target)
    {
        while (Vector2.Distance(obj.position, target) > 0.1f)
        {
            obj.position = Vector2.MoveTowards(obj.position, target, 5f * Time.deltaTime);
            yield return null;
        }

       
    }

    IEnumerator Phase2ProjectileRoutine()
    {
        while (currentPhase == BossPhase.Phase2 && !isDead)
        {
            if (!isAttacking && !isRadialAttacking)
            {
                Debug.Log("Phase2: Shooting projectiles");
                yield return ShootAtPlayer(); 
            }

            yield return new WaitForSeconds(5f); // Wait seconds between attacks
        }
    }
    
    IEnumerator TeleportBossAndBarrier()
    {
        if (teleportPositions == null || teleportPositions.Length == 0)
            yield break;

        
        // Add a sigil for the boss teleporting plus disappearing animation 
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
        barrierObject.SetActive(false);

        yield return new WaitForSeconds(teleportDelay);

        // Pick a random teleport position
        Transform target = teleportPositions[Random.Range(0, teleportPositions.Length)];

        transform.position = target.position;

        // Move the barrier with the boss
        barrierObject.transform.position = target.position;

        yield return new WaitForSeconds(0.1f);

        // Reappear
        if (sr != null) sr.enabled = true;
        barrierObject.SetActive(true);

        Debug.Log("Boss teleported to new position!");
    }
    
}
