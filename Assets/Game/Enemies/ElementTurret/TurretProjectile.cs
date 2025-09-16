using System;
using System.Collections;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public Transform player;
    public Transform firePoint;
    public GameObject projectilePrefab;
   // public GameObject lightningProjectilePrefab;
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

    
    private void Update()
    {
        if (player == null)
        {
            return;
        }
        
        float distance = Vector2.Distance(transform.position, player.position);
        
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
        }
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
  
    
    
}
