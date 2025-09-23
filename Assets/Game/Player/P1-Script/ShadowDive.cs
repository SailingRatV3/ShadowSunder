using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class ShadowDive : MonoBehaviour
{
    [Header("Input")] 
    public InputAction shadowDiveAction;
   
    [Header("Animator")] private Animator animator; 
    private static readonly int ShadowDiveTrigger = Animator.StringToHash("shadowDive"); 
    private static readonly int ExitShadowTrigger = Animator.StringToHash("exitShadow"); 
   
    [Header("State")] private bool isInShadow = true; // For testing — replace with actual trigger logic
    private bool isShadowForm = false; 
    private bool wasHoldingE = false; 
    private bool ignoreNextExit = false; 
    
    [Header("Settings")] 
    [SerializeField] private Collider2D lightCollider; // The raycasting if player is in the light
    
    [Header("Lights")] 
    [SerializeField] private float lightDetectionThreshold = 0.1f; // sensitivity light detect
    [SerializeField] private string lightTag = "LightSource"; // Light2D Tag private
    List<Light2D> detectedLights = new List<Light2D>();

    private bool IsPlayerInShadow()
    {
        return true; // In shadow unless enter light
    }

    private void OnEnable()
    {
        shadowDiveAction.Enable();
    }

    private void OnDisable()
    {
        shadowDiveAction.Disable();
    } 
    private void Awake()
    {
      //  shadowDiveAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/leftShift");
    }
    private void Start() { 
        
        animator = GetComponent<Animator>(); // Cache all Light2D sources with the given tag
        GameObject[] lightObjects = GameObject.FindGameObjectsWithTag(lightTag);
        foreach (GameObject lightObj in lightObjects)
        {
            Light2D light = lightObj.GetComponent<Light2D>();
            if (light != null)
            {
                detectedLights.Add(light);
            }
        } }

    private void Update()
    {
        float rawInput = shadowDiveAction.ReadValue<float>();
        bool isHoldingE = rawInput > 0.1f;
        bool isInLight = IsInLightRange();

       // Debug.Log($"[STATE] InLight: {isInLight} | ShadowForm: {isShadowForm} | KeyHeld: {isHoldingE}");

        // Exit if currently in shadow dive and now in light
        if (isShadowForm && isInLight)
        {
            Debug.Log("[LIGHT] Player entered light while in Shadow Form");
            PushPlayerBackFromLight();
            ExitShadowForm();
            wasHoldingE = isHoldingE;
            return;
        }

        // Enter shadow form if not already in it and not in light
        if (isHoldingE && !isShadowForm && !isInLight)
        {
            Debug.Log("[INPUT] Entering Shadow Form");
            EnterShadowForm();
        }

        // Exit on key release
        if (!isHoldingE && wasHoldingE && isShadowForm)
        {
            Debug.Log("[INPUT] Released key, exiting Shadow Form");
            ExitShadowForm();
        }

        wasHoldingE = isHoldingE;
    } 
    private void EnterShadowForm() { 
        Debug.Log("[STATE] Entering Shadow Form"); 
        isShadowForm = true; ignoreNextExit = true; 
        animator.ResetTrigger(ExitShadowTrigger); // Extra safe
        animator.SetTrigger(ShadowDiveTrigger); 
        // Player -> PlayerShadow 
        StartCoroutine(EnableExitDetection(0.1f)); }

    private IEnumerator EnableExitDetection(float Delay)
    {
        yield return new WaitForSeconds(Delay); ignoreNextExit = false;
    } private void ExitShadowForm() { 
        Debug.Log("[STATE] Exiting Shadow Form"); 
        isShadowForm = false; 
        animator.SetTrigger(ExitShadowTrigger); 
        // Shadow Layer -> Player 
    } 
    private bool IsInLightRange() { 
        Vector2 playerPosition = transform.position;
        foreach (Light2D light in detectedLights)
        {
            if (light == null || light.intensity <= 0f) continue; 
            Vector2 lightPosition = light.transform.position; 
            float distance = Vector2.Distance(playerPosition, lightPosition);
            switch (light.lightType)
            {
                case Light2D.LightType.Point: 
                case Light2D.LightType.Freeform: 
                case Light2D.LightType.Parametric:
                    if (distance <= light.pointLightOuterRadius)
                    {
                        float lightStrength = 1f - (distance / light.pointLightOuterRadius);
                        if (lightStrength * light.intensity > lightDetectionThreshold)
                        {
                            return true;
                        }
                    } 
                    break; 
               /* case Light2D.LightType.Global:
                    if (light.intensity > lightDetectionThreshold)
                    {
                        return true;
                    } 
                    break;*/
            }
        } 
        return false; 
    } 
    
    // Call this after spawning a new light

    public void RefreshLightList()
    {
        detectedLights.Clear(); 
        GameObject[] lightObjects = GameObject.FindGameObjectsWithTag(lightTag);
        foreach (GameObject lightObj in lightObjects)
        {
            Light2D light = lightObj.GetComponent<Light2D>();
            if (light != null)
            {
                detectedLights.Add(light);
            }
        }
    }

    private void PushPlayerBackFromLight()
    {
        GameObject[] lights = GameObject.FindGameObjectsWithTag(lightTag); 
        GameObject closestLight = null; float closestDistance = float.MaxValue;
        
        foreach (GameObject lightObj in lights)
        {
            float dist = Vector2.Distance(transform.position, lightObj.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist; closestLight = lightObj;
            }
        } 
        if (closestLight != null) { 
            Vector2 pushDir = (transform.position - closestLight.transform.position).normalized; 
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(pushDir * 15f, ForceMode2D.Impulse);
            } 
            else { transform.position += (Vector3)(pushDir * 0.1f); }
            Debug.Log("[LIGHT] Pushed player away from light!"); }
    } 
   
    }
    

