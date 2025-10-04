using System;
using System.Collections;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform firePoint;
    public GameObject projectilePrefab;
   // public GameObject lightningProjectilePrefab;
   public DetectionZone detectionZone;
   DamageableCharacters damageableCharacter;
   Rigidbody2D rb;
   Collider AlertCollider;
    Collider FireCollider;
    // public int damage = 2;
    Animator animator;
    [Header("Ranges")]
    public float wakeUpRange = 10f;
    public float fireRange= 6f;
    
    [Header("Fire Settings")]
    public float fireRate = 5f;
    public float fireCooldown = 1.5f;
    
    private float fireCooldownTimer = 0f;
    
    bool isAlerted = false;
    bool isAttacking = false;
    bool isSleeping = false;
    private bool isAwake = false;
    private bool isShooting = false;

    private float playerDMG = 1f;
    private float knockbackForce = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageableCharacter = GetComponent<DamageableCharacters>();
        animator = GetComponent<Animator>();
    }

    

   private void Update()
   {
       if (player == null || !damageableCharacter.Targetable)
           return;

       float distanceToPlayer = Vector2.Distance(transform.position, player.position);

       // Wake up if player is within wakeUpRange
       if (distanceToPlayer <= wakeUpRange)
       {
           if (!isAwake)
           {
               isAwake = true;
               OnWakeUp();
           }

           // Start shooting if player is close enough
           if (distanceToPlayer <= fireRange)
           {
               isShooting = true;
               HandleShooting();
           }
           else
           {
               isShooting = false;
           }
       }
       else
       {
           if (isAwake)
           {
               isAwake = false;
               isShooting = false;
               OnSleep();
           }
       }
   }

    void Shoot()
    {
        animator.SetBool("isFiring", true);
        Vector2 direction = (player.position - firePoint.position).normalized;
    
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
    
        ProjectileControl projectileScript = projectile.GetComponent<ProjectileControl>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
        }
        // Sound
    }
  
    private void HandleShooting()
    {
        fireCooldownTimer -= Time.deltaTime;

        if (fireCooldownTimer <= 0f)
        {
            Shoot();
            fireCooldownTimer = fireCooldown;
        }
        animator.SetBool("isFiring", false);
    }
    private void OnWakeUp()
    {
        // Play wake up animation, change color, sound, etc.
        animator.SetBool("isAwake", true);
        Debug.Log("Turret is now awake!");
    }

    private void OnSleep()
    {
        // Play sleep animation, change color back, sound, etc.
        animator.SetBool("isAwake", false);
        Debug.Log("Turret went back to sleep.");
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
                  
            damageable.OnHit(playerDMG, knockback); // implement OnHit

        }
    }
    
    private void OnDrawGizmos()
    {
        // Wake-up range
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f); // Yellow (semi-transparent)
        Gizmos.DrawWireSphere(transform.position, wakeUpRange);

        // Fire range
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // Red (semi-transparent)
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }
    
}
