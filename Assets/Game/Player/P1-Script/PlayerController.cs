using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // VAR
    
     GrappleClaw gc;
     SwordHitbox swordHitboxScript;
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
    [Header(("Movement"))]
    public float moveSpeed = 500f;
    public float maxSpeed = 8f;
    
    public float idleFriction = 0.9f;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 moveInput = Vector2.zero;
    
    bool isMoving = false;
    bool canMove = true;
    
    
    private bool isSwingRightToLeft = true;
    private bool isSwinging = false;
    private bool queuedSwing = false;
    
    private int comboStep = 0;
    private float comboResetTimer = 0f;
    public float comboResetTime = 1.0f;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] swordSounds;
   

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       gc = gameObject.GetComponent<GrappleClaw>();
       animator = GetComponent<Animator>();
       spriteRenderer = GetComponent<SpriteRenderer>();
       swordCollider = swordHitbox.GetComponent<Collider2D>();
       swordHitboxScript = swordHitbox.GetComponent<SwordHitbox>();
    }

    private void Update()
    {
        if (comboStep > 0)
        {
            comboResetTimer -= Time.deltaTime;
            if (comboResetTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

    [Obsolete("Obsolete")]
    void FixedUpdate()
    {
            if(canMove == true && moveInput != Vector2.zero)
            {
                // Move Anim + Velocity
                
                // Moving Accelerates Player
                // IMPORTANT: Acceleration max <= Max Speed
              
                rb.AddForce(moveInput * (moveSpeed * Time.deltaTime));
                if (rb.velocity.magnitude > maxSpeed)
                {
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
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, idleFriction);
                IsMoving = false;
            }            
            

        
            
        }
       
    
    // Input Values
    void OnMove(InputValue value)
    {
      moveInput = value.Get<Vector2>();
    }
    
    // play attack animation
    void OnFire()
    {
        if (isSwinging)
        {
            if(comboStep < 2)
             queuedSwing = true;
            return;
        }

        StartSwing();
    }

    

   public void LockMovement()
    {
        canMove = false;
    }

   public void UnlockMovement()
    {
        canMove = true;
    }
   

  void StartSwing()
  {
      isSwinging = true;
      
      SwordHitbox.AttackDirection attackDir;
      if (moveInput.y > 0.5f)
      {
          attackDir = SwordHitbox.AttackDirection.Up;
      }else if (moveInput.y < -0.5f)
      {
          attackDir = SwordHitbox.AttackDirection.Down;

      }else if (spriteRenderer.flipX)
      {
          attackDir = SwordHitbox.AttackDirection.Left;
      }
      else
      {
          attackDir = SwordHitbox.AttackDirection.Right;
      }
      swordHitboxScript.SetAttackDirection(attackDir);
      swordHitboxScript.StartAttack();
      SoundFXManager.instance.PlayRandomSoundFXClip(swordSounds, this.transform, 0.5f);   
      // Alternate Swing Animation
      if (comboStep == 0)
      {
          animator.SetTrigger("swordAttack");
          isSwingRightToLeft = false;
      }
      else if (comboStep == 1)
      {
          animator.SetTrigger("swordAttackAgain");
          isSwingRightToLeft = true;
      }

      comboStep++;
      comboResetTimer = comboResetTime;
      
  }
  void EndSwing()
  {
      isSwinging = false;

      if (comboStep >= 2)
      {
          ResetCombo();
          return;
      }
      
      if (queuedSwing)
      {
          queuedSwing = false;
          StartSwing(); 
          
      }
  }
  void ResetCombo()
  {
      comboStep = 0;
      comboResetTimer = 0f;
      queuedSwing = false;
      isSwingRightToLeft = true;
  }
}
