using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public Text healthText;
    public float maxHealth = 3f;
    private float lerpSpeed = 0.05f;
    private void Update()
    {
        if (healthSlider.value != _health)
        {
            healthSlider.value = _health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, _health, lerpSpeed);
        }
    }

    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }

    public bool Invincible { get; set; }

    public bool Targetable { get; set; }

    public float _health = 3f;
    public bool _targetable = true;
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
        throw new System.NotImplementedException();
    }
}
