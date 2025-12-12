using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("UI")]
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public TextMeshProUGUI healthText;
    [Header("Health Settings")]
    public float maxHealth = 3f;
    private float lerpSpeed = 0.05f;
    public float HealthPercent => damageableCharacter.Health / maxHealth;
    public bool IsFullHealth => damageableCharacter.Health >= maxHealth;
    
    [SerializeField] private PlayerRespawnManager playerRespawnManager;    
    [SerializeField] private DamageableCharacters damageableCharacter;

    private void Start()
    {
        damageableCharacter = GetComponent<DamageableCharacters>();
        if (damageableCharacter != null)
        {
            maxHealth = damageableCharacter.Health;  
        }

        if (playerRespawnManager == null)
            playerRespawnManager = FindObjectOfType<PlayerRespawnManager>();
    }

    private void Update()
    {
        if (damageableCharacter == null) return;

        if (damageableCharacter.Health <= 0)
        {
            Die();
        }

        if (damageableCharacter != null)
        {
            healthSlider.value = damageableCharacter.Health ;
            
            healthText.text = Mathf.RoundToInt(damageableCharacter.Health).ToString() + " / " + maxHealth.ToString();
            
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
        playerRespawnManager.OnPlayerDeath();
        
    }

    public void Heal(float healAmount)
    {
        damageableCharacter.Health += healAmount;
        damageableCharacter.Health = Mathf.Min(damageableCharacter.Health, maxHealth);
        
    }
    
    private void Die()
    {
        if (playerRespawnManager != null)
        {
            playerRespawnManager.OnPlayerDeath();
        }
        
    }
}
