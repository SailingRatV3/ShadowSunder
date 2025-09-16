using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShadowDive : MonoBehaviour
{
     [Header("Input")]
    public InputAction shadowDiveAction;  // Direct assignment

    [Header("Animator")]
    private Animator animator;
    private static readonly int ShadowDiveTrigger = Animator.StringToHash("shadowDive");
    private static readonly int ExitShadowTrigger = Animator.StringToHash("exitShadow");

    [Header("State")]
    private bool isInShadow = true; // For testing — replace with actual trigger logic
    private bool isShadowForm = false;
    private bool wasHoldingE = false;
    private bool ignoreNextExit = false;

    private bool IsPlayerInShadow()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("ShadowZone"));
        return hit != null;
    }
    private void OnEnable()
    {
        shadowDiveAction.Enable();
    }

    private void OnDisable()
    {
        shadowDiveAction.Disable();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool isHoldingE = shadowDiveAction.ReadValue<float>() > 0.1f;
        bool currentlyInShadow = IsPlayerInShadow();
        if (isHoldingE && !wasHoldingE)
        {
            Debug.Log("[INPUT] E Pressed");

            if (isInShadow && !isShadowForm)
            {
                Debug.Log("Enter Shadow Form");
                EnterShadowForm();
            }
        }

        if (!isHoldingE && wasHoldingE)
        {
            Debug.Log("[INPUT] E Released");

            if (isShadowForm)
            {
                Debug.Log("Exit Shadow Form");
                ExitShadowForm();
            }
        }

        if (!isInShadow && isShadowForm)
        {
            Debug.Log("No Longer in shadow zone while shadow dive");
            ExitShadowForm();
        }

        wasHoldingE = isHoldingE;
    }

    private void EnterShadowForm()
    {
        Debug.Log("[STATE] Entering Shadow Form");
        isShadowForm = true;
        ignoreNextExit = true;
        animator.ResetTrigger(ExitShadowTrigger); // Extra safe
        animator.SetTrigger(ShadowDiveTrigger);
       // Player -> PlayerShadow Layer
       gameObject.layer = LayerMask.NameToLayer("PlayerShadow");
        StartCoroutine(EnableExitDetection(0.1f));
    }

    private IEnumerator EnableExitDetection(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        ignoreNextExit = false;
    }

    private void ExitShadowForm()
    {
        Debug.Log("[STATE] Exiting Shadow Form");
        isShadowForm = false;
        animator.SetTrigger(ExitShadowTrigger);
       // Shadow Layer -> Player Layer
       gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShadowZone"))
        {
            
            isInShadow = true;
            Debug.Log("[ZONE] Entered ShadowZone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ShadowZone"))
        {           
           
            
            if (ignoreNextExit)
            {
                Debug.Log("[ZONE] ignore Exited ");
                return;
            }
            isInShadow = false;
            Debug.Log("[ZONE] Exited ShadowZone");

            if (isShadowForm)
            {
                ExitShadowForm();
            }
        }
    }
    }
    

