using System;
using UnityEngine;

public class DogEnemy : MonoBehaviour
{
    public float damage = 1;
    public float knockbackForce = 300f;
    public float movementSpeed = 500f;
    public DetectionZone detectionZone;
     Rigidbody2D rb;
    DamageableCharacters damageableCharacter;
     void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
        damageableCharacter = GetComponent<DamageableCharacters>();
    }

    void FixedUpdate()
    {            
       // Collider2D detectedObject0 = detectionZone.detectedObjects[0];

        if (damageableCharacter.Targetable && detectionZone.detectedObjects.Count > 0)
        {
            // Calc directiong to target 0 
            Vector2 direction = (detectionZone.detectedObjects[0].transform.position - transform.position).normalized;
            
            // move to first detected object
            rb.AddForce(direction * (movementSpeed * Time.deltaTime));
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
            Vector2 direction = (Vector2) (collider.gameObject.transform.position - transform.position).normalized; // normalized to not change the magnitude
            Vector2 knockback = direction * knockbackForce;
                  
            damageable.OnHit(damage, knockback); // implement OnHit

        }
    }
    
}