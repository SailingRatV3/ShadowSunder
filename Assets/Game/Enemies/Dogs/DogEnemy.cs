using System;
using UnityEngine;

public class DogEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    [Header("Attack")]
    public float damage = 1;
    public float knockbackForce = 300f;
    [Header("Movement")]
    public float movementSpeed = 500f;
    [Header("Ranges")]
    public float wakeUpRange = 10f;
   
    bool isAlerted = false;
    bool isAttacking = false;
    bool isSleeping = false;
    private bool isAwake = false;
    private bool isShooting = false;
    public DetectionZone detectionZone;
     Rigidbody2D rb;
    DamageableCharacters damageableCharacter;
    private SpriteRenderer spriteRenderer;
     void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();    
        damageableCharacter = GetComponent<DamageableCharacters>();
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
            else
                Debug.LogWarning("DogEnemy: Could not find player with tag 'Player'");
        }
    }

    void FixedUpdate()
    {            
        if (player == null || !damageableCharacter.Targetable)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
       
        if (distanceToPlayer <= wakeUpRange)
        {
           
            isAwake = true;
            Vector2 direction = (player.position - transform.position).normalized;
            rb.AddForce(direction * movementSpeed, ForceMode2D.Force);
            
            if (player.position.x < transform.position.x)
            {
                // Player is to the left
                if (spriteRenderer != null)
                    spriteRenderer.flipX = false; 
                else
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Player is to the right
                if (spriteRenderer != null)
                    spriteRenderer.flipX = true;
                else
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
          
            isAwake = false;
            rb.linearVelocity = Vector2.zero;
        }
       
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Collider2D collider = col.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            
            // Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;
            Vector3 parentPosition = transform.position; // Get sprite orgin position
            Vector2 direction = (Vector2) (collider.gameObject.transform.position - transform.position).normalized; 
            Vector2 knockback = direction * knockbackForce;
                  
            damageable.OnHit(damage, knockback); // implement OnHit

        }
    }
    
    
    private void OnDrawGizmos()
    {
        // Wake-up range
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f); // Yellow (semi-transparent)
        Gizmos.DrawWireSphere(transform.position, wakeUpRange);

    }
    
}