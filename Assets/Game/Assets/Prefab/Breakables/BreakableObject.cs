using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakableObject : MonoBehaviour, IDamageable
{
    [Header("FX")]
    [SerializeField] private ParticleSystem psDestroy;
    [SerializeField] private ParticleSystem psHit;
    private Vector2 lastHitDirection = Vector2.down; // Default
    
    [Header("Knockback")]
    public float knockbackForce = 5f;
    public bool hasKnockback = false;
    public float knockbackDuration = 0.2f;
    private Coroutine knockbackRoutine;
    Rigidbody2D rb;
    
    [Header("HP")]
    public float hitPoints = 2;
    
    [Header("Item Drop")]
    public GameObject itemDropPrefab;
    public bool hasItemInside = false;
    public float maxDropChance = 0.5f;
    public float minDropChance = 0.1f;
    public float Health { set
        {
            
            hitPoints = value;

            if (hitPoints < 0)
            {
                hitPoints = 0;
            }
            
           

            if (hitPoints > 0)
            {
                Debug.Log("Triggering hit animation");
                // Make Hit Animation: player hit animation
                // animator.SetTrigger("hit");
                PlayDirectionalParticles(psHit,lastHitDirection);
                GetComponent<DamageFlash>().Flash();
            }
            

            

            if (hitPoints <= 0)
            {
                Debug.Log("Health is zero. Setting isAlive = false");
                hitPoints = 0;
                // isAlive = false;
                // Make Death Animation: play animation before destroy
               // animator.SetBool("isAlive", false);
               PlayParticles(psDestroy);
               // Targetable = false; 
                //Destroy(gameObject);
                OnObjectDestroyed();
            }
        }
        get => hitPoints;
        
    }
    public bool Invincible { get; set; }
    public bool Targetable { get; set; }
    public void OnHit(float damage, Vector2 knockback)
    {
        lastHitDirection = knockback.normalized;
        Health -= damage;
        if (hasKnockback)
        {
            if (knockbackRoutine != null)
                StopCoroutine(knockbackRoutine);

            knockbackRoutine = StartCoroutine(ApplyKnockback(knockback.normalized * knockbackForce));

        }
    }

    public void OnHit(float damage)
    {
        lastHitDirection = Vector2.down; // Default direction
        Health -= damage;
    }

    private void DropItemBasedOnPlayerHealth()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null || itemDropPrefab == null)
        {
            return;
        }
        
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if(playerHealth == null)
            return;

        float healthPercent = playerHealth.HealthPercent;
        float dropChance = Mathf.Lerp(maxDropChance, minDropChance, healthPercent);

        if (Random.value < dropChance)
        {
            Instantiate(itemDropPrefab, player.transform.position, Quaternion.identity);
        }
    }
    public void OnObjectDestroyed()
    {
        if (hasItemInside && itemDropPrefab != null)
        {
            Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
            
        }
        
        Destroy(gameObject); 
    }

    void PlayParticles(ParticleSystem ps)
    {
        ParticleSystem effect = Instantiate(ps, transform.position, transform.rotation);
       effect.Emit(1);
       // effect.Play(); 
      //  Destroy(this.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
    }

    void PlayDirectionalParticles(ParticleSystem ps, Vector2 direction)
    {
       Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction.normalized);
        ParticleSystem effect = Instantiate(ps, transform.position, rotation);
        effect.Emit(1);
    }
    private IEnumerator ApplyKnockback(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
