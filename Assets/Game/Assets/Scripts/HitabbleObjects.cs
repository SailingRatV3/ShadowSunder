using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitabbleObjects : MonoBehaviour, IDamageable
{
    public float knockbackforce = 5f;
    public float knockbackDuration = 0.2f;
    private Coroutine knockbackRoutine;
    
    public float Health { get; set; }
    public bool Invincible { get; set; }
    public bool Targetable { get; set; }
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        if (knockbackRoutine != null)
            StopCoroutine(knockbackRoutine);

        knockbackRoutine = StartCoroutine(ApplyKnockback(knockback.normalized * knockbackforce));
    }

    public void OnHit(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void OnObjectDestroyed()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator ApplyKnockback(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
    }
    
}
