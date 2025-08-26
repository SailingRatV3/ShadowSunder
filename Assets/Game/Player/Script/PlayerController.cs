using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // VAR
     GrappleClaw gc;
    public GameObject swordHitbox;
    Collider2D swordCollider;
    bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    
    public float moveSpeed = 500f;
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
       gc = gameObject.GetComponent<GrappleClaw>();
       animator = GetComponent<Animator>();
       spriteRenderer = GetComponent<SpriteRenderer>();
       swordCollider = swordHitbox.GetComponent<Collider2D>();
    }

    [Obsolete("Obsolete")]
    void FixedUpdate()
    {
        //if (!gc.retracting)
        //{
            if(canMove == true && moveInput != Vector2.zero)
            {
                // Move Anim & +Velocity
                
                // Moving Accelerates Player
                // IMPORTANT: Acceleration max <= Max Speed
                // rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity + (moveInput * moveSpeed * Time.deltaTime), maxSpeed);
                rb.AddForce(moveInput * (moveSpeed * Time.deltaTime));
                if (rb.velocity.magnitude > maxSpeed)
                {
                   // rb.velocity = Vector2.Lerp(rb.velocity, maxSpeed, idleFriction);
                    float limitSpeed = Mathf.Lerp(rb.velocity.x, maxSpeed, idleFriction);
                    rb.velocity = rb.velocity.normalized * limitSpeed;
                }
                // Looking Left or Right
                if (moveInput.x > 0)
                {
                    spriteRenderer.flipX = false;
                    gameObject.BroadcastMessage("IsFacingRight", true);
                }else if (moveInput.x < 0)
                {
                    spriteRenderer.flipX = true;
                    gameObject.BroadcastMessage("IsFacingRight", false);
                }
                IsMoving = true;
            }
            else
            {
                // No Movement
               // Don't need this line if using angular damp
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, idleFriction);
                IsMoving = false;
            }            
            // UpdateAnimatorParameters();

        
            
        }
       // else
        //{
            //rb.velocity = Vector2.zero;
        //}
   // }
    
    // Input Values
    void OnMove(InputValue value)
    {
      moveInput = value.Get<Vector2>();
    }
    
    // play attack animation
    void OnFire()
    {
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
