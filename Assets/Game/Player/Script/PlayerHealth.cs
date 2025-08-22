using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
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
