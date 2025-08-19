using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // VAR
    
    bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    
    public float moveSpeed = 150f;
    public float maxSpeed = 8f;
    
    // VAR for the frame of physics that will shave off the velocity
    public float idleFriction = 0.9f;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 moveInput = Vector2.zero;
    
    bool isMoving = false;
    bool canMove = true;

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(canMove == true && moveInput != Vector2.zero)
        {
            // Move Anim & +Velocity
            
            // Moving Accelerates Player
            // IMPORTANT: Acceleration max <= Max Speed
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity + (moveInput * moveSpeed * Time.deltaTime), maxSpeed);
            
            // Looking Left or Right
            if (moveInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }else if (moveInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            IsMoving = true;
        }
        else
        {
            // No Movement
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, idleFriction);
            IsMoving = false;
        }            
        // UpdateAnimatorParameters();

    }
    
    // Input Values
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    // play attack animation
    void OnFire()
    {
        // Vid : 13:31
        animator.SetTrigger("swordAttack");
    }

    void LockMovement()
    {
        canMove = false;
    }

    void UnlockMovement()
    {
        canMove = true;
    }
  /*  void UpdateAnimatorParameters()
    {
        // animator.SetFloat("moveX", moveInput.x);
       // animator.SetFloat("moveY", moveInput.y);
       animator.SetBool("isMoving", isMoving);
    }*/
}
