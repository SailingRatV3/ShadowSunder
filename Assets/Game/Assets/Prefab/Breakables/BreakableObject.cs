using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageable
{
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
                
            }
            

            

            if (hitPoints <= 0)
            {
                Debug.Log("Health is zero. Setting isAlive = false");
                hitPoints = 0;
                // isAlive = false;
                // Make Death Animation: play animation before destroy
               // animator.SetBool("isAlive", false);
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
        Health -= damage;
    }

    public void OnHit(float damage)
    {
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
}
