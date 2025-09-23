using UnityEngine;
using System;
public class ColliderTrigger : MonoBehaviour
{
    // Use this in Colliders to trigger events 
    
    public event EventHandler OnPlayerTriggerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            // Player Inside
            OnPlayerTriggerEnter?.Invoke(this, EventArgs.Empty);
        }
    }
}
