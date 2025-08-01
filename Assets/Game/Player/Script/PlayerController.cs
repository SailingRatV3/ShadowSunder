using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // VAR
    public float moveSpeed = 150f;
    public float maxSpeed = 8f;
    
    // VAR for the frame of physics that will shave off the velocity
    public float idleFriction = 0.9f;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 moveInput = Vector2.zero;

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(moveInput != Vector2.zero)
        {
            spriteRenderer.flipX = moveInput.x > 0;
        }
    }
    
}
