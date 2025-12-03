using System;
using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    public float speed = 10f;
    public float knockbackForce = 10f;
    public float damage = 1f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    [SerializeField] private AudioClip[] hitSound;

    // This method will be called by the turret to set the direction.
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;  // Make sure it's normalized to prevent varying speeds.
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    
    public float lifetime = 5f;

   
        
    
    private void Start()
    {
        // If you're using Rigidbody2D, apply the velocity here.
        if (rb != null && moveDirection != Vector2.zero)
        {
            rb.linearVelocity = moveDirection * speed;
        }
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Only hit if it's the player
        if (col.CompareTag("Player"))
        {
            IDamageable damageable = col.GetComponent<IDamageable>();
            if (damageable != null)
            {
                SoundFXManager.instance.PlayRandomSoundFXClip(hitSound, this.transform, 0.2f);
                Vector2 direction = (Vector2)(col.transform.position - transform.position).normalized;
                Vector2 knockback = direction * knockbackForce;
                damageable.OnHit(damage, knockback);
            }

            Destroy(gameObject); // Destroy projectile on impact
        }
    }
}
