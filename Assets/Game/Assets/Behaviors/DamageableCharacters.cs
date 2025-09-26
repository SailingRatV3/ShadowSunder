using System;
using UnityEngine;

public class DamageableCharacters : MonoBehaviour, IDamageable
{
    [Header("Damageable Characters Settings")]
     public bool disableSimulation = false;
     public float invincibilityTime = 0.25f;
     public bool isInvinciblityEnable = false;
    Animator animator;
    Rigidbody2D rb;
    Collider2D physicsCollider;
    bool isAlive = true;
    private float invincibilityTimeElapse = 0f;
    [Header("Knockback Settings")]
    public bool canRecieveKnockback = true;
    public event Action OnDamage;
    public event Action OnDeath;

    public void TakeDamage(float damage)
    {
        Health -= damage;
        OnDamage?.Invoke();
        if (Health <= 0)
        {
            OnDeath?.Invoke();
        }
    }
    
    public float Health
    {
        set
        {
            Debug.Log($"[Health Setter] Setting Health to {value}, current _health = {_health}");
            _health = value;

            if (_health < 0)
            {
                _health = 0;
            }
            
            Debug.Log($"{gameObject.name} new Health: {_health}");

            if (_health > 0)
            {
                Debug.Log("Triggering hit animation");
                // Make Hit Animation: player hit animation
                animator.SetTrigger("hit");
               // animator.SetBool("isAlive", true);
            }
            

            

            if (_health <= 0)
            {
                Debug.Log("Health is zero. Setting isAlive = false");
                _health = 0;
                isAlive = false;
               // Make Death Animation: play animation before destroy
              animator.SetBool("isAlive", false);
              Targetable = false; 
              //Destroy(gameObject);
                
            }
        }
        get => _health;
    }

    public bool Invincible
    {
        get
        {
            return _invincible;
        } set
        {
           _invincible = value;
           if (_invincible == true)
           {
               invincibilityTimeElapse = 0f;
           }
        }
    }

    public bool Targetable
    {
        get { return _targetable;}
        set
        {
            _targetable = value; 
            if (disableSimulation)
            {
               rb.simulated = false; // if targetable turn on phsyics // Can commit this off if want the enemy death to still move
                            
            }
            physicsCollider.enabled = value; // the last hit will eeffect the object
        }
    }

   public float _health = 3;
    public float _maxHealth = 3;
     bool _targetable = true;
     bool _invincible = false;
    public void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", true);
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
    }
    
// Interface
    public void OnHit(float damage, Vector2 knockback)
    {
        Debug.Log($"[OnHit] {gameObject.name} took {damage} damage (vector knockback)");
        if (!Invincible)
        {
           Health -= damage; 
           // apply knockback force
           if (canRecieveKnockback)
           {
               rb.AddForce(knockback, ForceMode2D.Impulse);

           }

           if (isInvinciblityEnable)
           {
               // activate invincibility + timer
               Invincible = true;
               
           }
        }
        
        
        //Debug.Log("Force : " + knockback);
    }

    public void OnHit(float damage)
    {
        Debug.Log($"[OnHit] {gameObject.name} took {damage} damage (scalar)");
        if (!Invincible)
        {
            //Debug.Log("Dog Hit " + damage);
            Health -= damage;
            
            if (isInvinciblityEnable)
            {
                // activate invincibility + timer
                Invincible = true;
               
            }
        }
    }

    public void OnObjectDestroyed()
    {
        
            Debug.Log("Destroying enemy");
           Destroy(gameObject); 
        
        
    }

    public void FixedUpdate()
    {
        if (Invincible)
        {
            invincibilityTimeElapse += Time.deltaTime;

            if (invincibilityTimeElapse > invincibilityTime)
            {
                Invincible = false;
            }
        }
    }

    public void Die()
    {
        if (OnDeath != null)
        {
            OnDeath.Invoke();
        }
    }
    
}
