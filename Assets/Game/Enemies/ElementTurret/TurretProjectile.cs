using System;
using System.Collections;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
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

    public float wakeUpRange = 10f;
    public float fireRate = 5f;
    public float fireCooldown = 1.5f;
    
    private float fireCooldownTimer = 0f;
    
    bool isAlerted = false;
    bool isAttacking = false;
    bool isSleeping = false;

    private float playerDMG = 1f;
    private float knockbackForce = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageableCharacter = GetComponent<DamageableCharacters>();
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }
        
        
    }

    void FixedUpdate()
    {
        if (damageableCharacter.Targetable && detectionZone.detectedObjects.Count > 0)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            isAlerted = true;
            
            if (isAlerted)
            {
                if (distance <= fireRate)
                {
                    fireCooldownTimer -= Time.deltaTime;
                    if (fireCooldownTimer <= 0f)
                    {
                        Shoot();
                        fireCooldownTimer = fireRate;
                    }
                }
            }
    }

    /*
    // alerted Check
    if (distance < wakeUpRange)
    {
        isAlerted = true;
    }
    else
    {
        isAlerted = false;
    }

    if (isAlerted)
    {
        if (distance <= fireRate)
        {
            fireCooldownTimer -= Time.deltaTime;
            if (fireCooldownTimer <= 0f)
            {
                Shoot();
                fireCooldownTimer = fireRate;
            }
        }
        // Rotate Self to Player
        //Vector2 dir = (player.position - transform.position).normalized;
       // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
       // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }*/
    }

    void Shoot()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;
    
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
    
        ProjectileControl projectileScript = projectile.GetComponent<ProjectileControl>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
        }
        // Sound
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
}
