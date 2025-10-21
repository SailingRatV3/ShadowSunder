using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingBarrier : MonoBehaviour
{
    private HashSet<GameObject> playersInsideInner = new HashSet<GameObject>();
    
    [Header("Damage")]
    public float damage = 1f;
    public float knockbackForce = 100f;
    public float damageInterval = 1f;
    
    private Dictionary<GameObject, float> lastDamageTime = new Dictionary<GameObject, float>();
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playersInsideInner.Contains(other.gameObject))
        {
            GameObject player = other.gameObject;
            
            if (playersInsideInner.Contains(player))
                return;
            
            var shadowDive = player.GetComponent<ShadowDive>();
            if(shadowDive != null && shadowDive.IsDiving)
                return;
            
            // Damage player 
            if (!lastDamageTime.ContainsKey(player) || Time.time - lastDamageTime[player] > damageInterval)
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    Vector2 direction = (Vector2)(other.transform.position - transform.position).normalized;
                    Vector2 knockback = direction * knockbackForce;
                    damageable.OnHit(damage, knockback);
                    lastDamageTime[player] = Time.time;
                }
            }
        }
    }

    public void RegisterInner(Collider2D innerCollider)
    {
        if (innerCollider.CompareTag("Player"))
        {
            playersInsideInner.Add(innerCollider.gameObject);
        }
    }

    public void UnregisterInner(Collider2D innerCollider)
    {
        if (innerCollider.CompareTag("Player"))
        {
            playersInsideInner.Remove(innerCollider.gameObject);
        }
    }
}
