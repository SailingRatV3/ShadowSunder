using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System;
public class HealthPickup : MonoBehaviour, ICollectible
{
    [Header("SFX")]
    public ParticleSystem ps;
    [Header("Healing")]
    public float healAmount = 1f;
    
    public static event Action OnHealthCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (playerHealth.IsFullHealth)
                {
                    return;
                }
                
                playerHealth.Heal(healAmount);
                
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                ParticleSystem effect = Instantiate(ps, transform.position, transform.rotation);
                effect.Play();
                Destroy(this.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
                Collect();
                       
            }
            
           
            
        }
        
    }

    public void Collect()
    {
        Debug.Log(gameObject.name + " collected");
        Destroy(gameObject);
        OnHealthCollected?.Invoke();
    }
}
