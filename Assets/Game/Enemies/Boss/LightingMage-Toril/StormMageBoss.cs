using UnityEngine;

public class StormMageBoss : MonoBehaviour, IDamageable
{
    public float Health { get; set; }
    public bool Invincible { get; set; }
    public bool Targetable { get; set; }
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
