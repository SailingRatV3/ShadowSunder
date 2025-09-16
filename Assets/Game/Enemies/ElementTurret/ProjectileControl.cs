using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    // This method will be called by the turret to set the direction.
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;  // Make sure it's normalized to prevent varying speeds.
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    
    public float lifetime = 5f;

   
        
    
    private void Start()
    {
        // If you're using Rigidbody2D, apply the velocity here.
        if (rb != null && moveDirection != Vector2.zero)
        {
            rb.linearVelocity = moveDirection * speed;
        }
        Destroy(gameObject, lifetime);
    }
}
