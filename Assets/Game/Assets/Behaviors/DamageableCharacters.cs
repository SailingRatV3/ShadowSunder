using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableCharacters : MonoBehaviour, IDamageable
{
    
    [Header("Damageable Characters Settings")]
     public bool disableSimulation = false;
     public float invincibilityTime = 0.25f;
     public bool isInvinciblityEnable = false;
     private SpriteRenderer spriteRenderer;
     private Coroutine invincibilityFlashCoroutine;
    Animator animator;
    Rigidbody2D rb;
    Collider2D physicsCollider;
    bool isAlive = true;
    private float invincibilityTimeElapse = 0f;
    [Header("Knockback Settings")]
    public bool canRecieveKnockback = true;
    [Header("Audio Clips")]
   // [SerializeField] private AudioClip[] knockbackSounds;
    [SerializeField] private AudioClip[] damageSounds;
   // [SerializeField] private AudioClip[] deathSounds;
    public event Action OnDamage;
    public event Action OnDeath;
   
    [SerializeField] private CameraShakeManager cameraShakeManager;
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
            
            //Debug.Log($"{gameObject.name} new Health: {_health}");

            if (_health > 0)
            { 
                cameraShakeManager.TriggerShakeByName("Hit");

             
            
                animator.SetTrigger("hit");
                SoundFXManager.instance.PlayRandomSoundFXClip(damageSounds, this.transform, 0.5f);
                
            }
            

            

            if (_health <= 0)
            {
            
                _health = 0;
                isAlive = false;
              
               cameraShakeManager.TriggerShakeByName("Hit");
              animator.SetBool("isAlive", false);
              Targetable = false; 
              
                
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
               physicsCollider.enabled = false; // Disable hitbox

               if (invincibilityFlashCoroutine != null)
                   StopCoroutine(invincibilityFlashCoroutine);

               invincibilityFlashCoroutine = StartCoroutine(InvincibilityFlash());
           }
           else
           {
               physicsCollider.enabled = true; // Re-enable hitbox

               if (invincibilityFlashCoroutine != null)
               {
                   StopCoroutine(invincibilityFlashCoroutine);
                   invincibilityFlashCoroutine = null;
               }

               
               if (spriteRenderer != null)
               {
                   var color = spriteRenderer.color;
                   color.a = 1f;
                   spriteRenderer.color = color;
               }
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
               rb.simulated = false; // Can commit this off to make enemy death still animate
                            
            }
            physicsCollider.enabled = value; 
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
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
// Interface
    public void OnHit(float damage, Vector2 knockback)
    {
        //Debug.Log($"[OnHit] {gameObject.name} took {damage} damage (vector knockback)");
        if (!Invincible)
        {
           Health -= damage; 
           OnDamage?.Invoke(); 
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
        
    }

    public void OnHit(float damage)
    {
        Debug.Log($"[OnHit] {gameObject.name} took {damage} damage (scalar)");
        if (!Invincible)
        {
            //Debug.Log("Dog Hit " + damage);
            Health -= damage;
            OnDamage?.Invoke(); 
            
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
           if (gameObject.tag == "Player")
           {
               PlayerRespawnManager playerRespawnManager = gameObject.GetComponentInParent<PlayerRespawnManager>();
               playerRespawnManager.OnPlayerDeath();
           }
        
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
    
    private IEnumerator InvincibilityFlash()
    {
        float flashInterval = 0.1f;

        while (Invincible)
        {
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = (color.a == 1f) ? 0.2f : 1f; 
                spriteRenderer.color = color;
            }

            yield return new WaitForSeconds(flashInterval);
        }
    }
    
}
