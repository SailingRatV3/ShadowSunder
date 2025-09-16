using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageable
{
    public float hitPoints = 2;
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

    public void OnObjectDestroyed()
    {
        Destroy(gameObject); 
    }
}
