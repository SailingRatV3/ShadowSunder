using System;
using System.Collections;
using UnityEngine;

public class BossLightningStrike : MonoBehaviour
{
    Animator animator;

    // Don't do warning prefab and have the 
   // public GameObject warningPrefab;
    public GameObject lightningPrefab;
    public float warningDuration = 0.1f;
    public float strikeRadius = 1f;
    public int damage = 2;
    public float knockbackForce = 1f;
    bool isStriking = false;

     void Start()
    {
        animator = lightningPrefab.GetComponent<Animator>();
    }

    public void TriggerLightningStrike(Transform player)
    {
        if (!isStriking)
        {
            StartCoroutine(LightningStrikeCoroutine(player));
        }
    }

      public IEnumerator LightningStrikeCoroutine(Transform player)
    {
        isStriking = true;
        
        Vector3 targetPos = player.position;
        
        // Warning 
       // GameObject warning = Instantiate(warningPrefab, targetPos, Quaternion.identity);
        
       GameObject lightning = Instantiate(lightningPrefab, targetPos, Quaternion.identity);
        
       Animator lightningAnimator = lightning.GetComponent<Animator>();
        //Destroy(warning, warningDuration);
        yield return new WaitForSeconds(warningDuration);
        
        //animator.SetBool("AfterWarning", AfterWarning);
        // Lightning Strike
         

        if (lightningAnimator != null)
        {
            lightningAnimator.SetBool("AfterWarning", true);
        }
        
        // Are player in the Strike Zone
        float distance = Vector3.Distance(player.position, targetPos);
        if (distance <= strikeRadius)
        {
            // Apply damage to player
            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Vector2 direction = (Vector2)(player.transform.position - transform.position).normalized;
                Vector2 knockback = direction * knockbackForce;
                damageable.OnHit(damage, knockback);
            }
        }
        
        yield return new WaitForSeconds(warningDuration);
        Destroy(lightning);
        isStriking = false;
    }

}
