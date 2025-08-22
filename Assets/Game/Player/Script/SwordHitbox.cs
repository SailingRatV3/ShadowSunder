using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public float swordDamage = 1f;
    public float knockbackForce = 1000f;
   public Collider2D swordCollider;
   // Facing Direction
   public Vector3 faceRight = new Vector3(2.03f, 0f, 0f);
   public Vector3 faceLeft = new Vector3(-2.03f, 0f, 0f);
   
   private List<GameObject> hitEnemies = new List<GameObject>();
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
        // Optionally enable the collider here if you disable it outside attack time
        //swordCollider.enabled = true;
         StartCoroutine(EnableColliderForAttack());
    }

    public void EndAttack()
    {
        // Optionally disable collider to avoid stray hits
         swordCollider.enabled = false;
    }
    
    private IEnumerator EnableColliderForAttack()
    {
        swordCollider.enabled = true; // Enable the collider just before the attack.
        yield return null; // Wait for one frame to ensure physics engine catches the change.
        yield return new WaitForFixedUpdate(); // Wait for one fixed frame
        // Now the collider is properly enabled and ready to trigger again.
    }
    
}
