using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BreakableLights : MonoBehaviour, IDamageable
{
    [Header("HP")]
    public float hitPoints = 1;
    [Header("Light")]
    public Light2D light;


    void Awake()
    {
        if (light == null)
        {
                    light = GetComponent<Light2D>();

        }
    }
    
    public float Health { 
        get => hitPoints; 
        set
        {
            
            hitPoints = value;
            
            if (hitPoints == 0)
            {
                // Health = 0;
                TurnOffLight();
            }


        }
        
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
        throw new System.NotImplementedException();
    }
    
    private void TurnOffLight()
    {
        if (light != null)
        {
            light.intensity = 0f;
            light.enabled = false;
        }
        else
        {
            Debug.LogWarning("No Light2D component found on BreakableLight!");
        }

        // Add visual/sound effects
        // GetComponent<Animator>()?.SetTrigger("break");
    }
    
}
