using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CharacterHealth : MonoBehaviour, IDamageable
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    
    public float maxHealth = 3f;
    private float lerpSpeed = 0.05f;
    
    [SerializeField] private DamageableCharacters damageableCharacter;

    private void Start()
    {
        damageableCharacter = GetComponent<DamageableCharacters>();
        if (damageableCharacter != null)
        {
            maxHealth = damageableCharacter.Health;  
        }
    }

    private void Update()
    {
        if (damageableCharacter != null)
        {
            healthSlider.value = damageableCharacter.Health ;
            
            
            if (healthSlider.value != easeHealthSlider.value)
            {
                easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, _health, lerpSpeed);
            }
        }
    }

    public float Health
    {
        get => damageableCharacter != null ? damageableCharacter.Health : 0f;
        set
        {
            if (damageableCharacter != null)
            {
                damageableCharacter.Health = value;
            }
        }
    }

    public bool Invincible { get; set; }

    public bool Targetable { get; set; }

    public float _health = 3f;
   
    public void OnHit(float damage, Vector2 knockback)
    {
        throw new System.NotImplementedException();
    }

    public void OnHit(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
    }
}
