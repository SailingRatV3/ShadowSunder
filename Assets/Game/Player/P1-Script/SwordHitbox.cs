using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [Header("Sword Settings")]
    public float swordDamage = 1f;
    public float knockbackForce = 1000f;
   public Collider2D swordCollider;
   // Facing Direction
   [Header("Direction Offsets")]
   public Vector3 faceRight = new Vector3(2.03f, 0f, 0f);
   public Vector3 faceLeft = new Vector3(-2.03f, 0f, 0f);
   public Vector3 faceUp = new Vector3(0f, 2.03f, 0f);
   public Vector3 faceDown = new Vector3(0f, -2.03f, 0f);
   
   [Header("Rotation Angles")]
   public float upAngle = 90f;
   public float downAngle = -90f;
   public float leftAngle = 180f;
   public float rightAngle = 0f;

   
   
   private List<GameObject> hitEnemies = new List<GameObject>();
   
   public enum AttackDirection
   {
       Left,
       Right,
       Up,
       Down
   }
    void Start()
   {
     //  swordCollider = GetComponent<Collider2D>();
     if (swordCollider == null)
     {
         Debug.LogWarning("Sword Collider is not Set!");
     }
   }
    
    
    // Trigger Event
   void OnTriggerEnter2D(Collider2D collider)
    {
        //StartAttack();
        GameObject root = collider.transform.root.gameObject;
        // Debug.Log($"Hit detected on: {collider.name} (Root: {root.name})");
        if (hitEnemies.Contains(root)) return;
       // if (!root.CompareTag("Enemy")) return;
        hitEnemies.Add(root);
        StartAttack();
        //EndAttack();
       IDamageable damageableObject = collider.GetComponentInParent<IDamageable>();
       if (damageableObject != null)
       {
           
           
          // Calculating knockback force
                  // Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;
                  Vector3 parentPosition = transform.parent.position; // Get sprite orgin position
                  Vector2 direction = (collider.transform.position - parentPosition).normalized; // normalized to not change the magnitude
                  Vector2 knockback = direction * knockbackForce;
                  
                  //collider.SendMessage("OnHit", swordDamage, knockback); // Only work with x and y value
                  damageableObject.OnHit(swordDamage, knockback); // implement OnHit
                  
                  // Debug.Log("Hit"); 
       }
       else
       {
           // Check if object hit is implement or not
           Debug.LogWarning("Sword Collider does not implement IDamageable!");
       }
       
       
    }

    void IsFacingRight(bool isFacingRight)
    {
        if (isFacingRight)
        {
            gameObject.transform.localPosition = faceRight;
        }
        else
        {
            gameObject.transform.localPosition = faceLeft;
        }
    }

    public void StartAttack()
    {
        Debug.Log("StartAttack called: clearing hitEnemies list");
        hitEnemies.Clear();
        // enable the collider here if you disable it outside attack time
        //swordCollider.enabled = true;
         StartCoroutine(EnableColliderForAttack());
    }

    public void EndAttack()
    {
        // disable collider to avoid stray hits
         swordCollider.enabled = false;
    }
    
    private IEnumerator EnableColliderForAttack()
    {
        swordCollider.enabled = true; // Enable the collider just before the attack.
        yield return null; 
        yield return new WaitForFixedUpdate(); 
        
    }

    public void SetAttackDirection(AttackDirection dir)
    {
        switch (dir)
        {
            case AttackDirection.Right:
                transform.localPosition = faceRight;
                transform.localRotation = Quaternion.Euler(0,0, rightAngle);
                break;
            case AttackDirection.Left:
                transform.localPosition = faceLeft;
                transform.localRotation = Quaternion.Euler(0,0, leftAngle);
                break;
            case AttackDirection.Up:
                transform.localPosition = faceUp;
                transform.localRotation = Quaternion.Euler(0,0, upAngle);
                break;
            case AttackDirection.Down:
                transform.localPosition = faceDown;
                transform.localRotation = Quaternion.Euler(0,0, downAngle);
                break;
        }
    }
    
}
