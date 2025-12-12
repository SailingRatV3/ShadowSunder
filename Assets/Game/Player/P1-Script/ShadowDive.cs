using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ShadowDive : MonoBehaviour
{
    [Header("Input")] 
    public InputAction shadowDiveAction;
   
    [Header("Animator")] private Animator animator; 
    private static readonly int ShadowDiveTrigger = Animator.StringToHash("shadowDive"); 
    private static readonly int ExitShadowTrigger = Animator.StringToHash("exitShadow"); 
   
    [Header("State")] private bool isInShadow = true; // replace this with trigger logic
    private bool isShadowForm = false; 
    private bool wasHoldingE = false; 
    private bool ignoreNextExit = false;

    private Image gateImage;
    public bool IsDiving { get; private set; }

    public void StartDive()
    {
        IsDiving = true;
    }

    public void EndDive()
    {
        IsDiving = false;
    }
    
    [Header("Settings")] 
    [SerializeField] private Collider2D lightCollider; 

    [SerializeField] private Collider2D playerCollider;
    
    [Header("Lights")] 
    [SerializeField] private float lightDetectionThreshold = 0.1f; 
    [SerializeField] private string lightTag = "LightSource"; 
    List<Light2D> detectedLights = new List<Light2D>();

    [Header("Layers")]
    [SerializeField] private String normalLayerName = "Player";
    [SerializeField] private String shadowLayerName = "PlayerShadow";
    private int normalLayer;
    private int shadowLayer;
    
    
    private bool IsPlayerInShadow()
    {
        return true; 
    }

    private void OnEnable()
    {
        shadowDiveAction.Enable();
        shadowDiveAction.performed += OnShadowDivePressed;
    }

    private void OnDisable()
    {
        shadowDiveAction.performed -= OnShadowDivePressed;
        shadowDiveAction.Disable();
    } 
    
    private void Start() { 
        
        animator = GetComponent<Animator>(); 
       
        normalLayer = LayerMask.NameToLayer(normalLayerName);
        shadowLayer = LayerMask.NameToLayer(shadowLayerName);

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
       
        
        bool isInLight = IsInLightRange();

       
        if (isShadowForm && isInLight)
        {
            PushPlayerBackFromLight();
            ExitShadowForm();
           
        }

        

        
    } 
    private void EnterShadowForm()
    {
        StartDive();
        SetLayerRecursively(gameObject, shadowLayer);
        playerCollider.enabled = false;
        
        isShadowForm = true; ignoreNextExit = true; 
        animator.ResetTrigger(ExitShadowTrigger); 
        animator.SetTrigger(ShadowDiveTrigger); 
        // Player -> PlayerShadow 
        StartCoroutine(EnableExitDetection(0.1f)); }

    private IEnumerator EnableExitDetection(float Delay)
    {
        yield return new WaitForSeconds(Delay); ignoreNextExit = false;
    } private void ExitShadowForm()
    {
        EndDive();
        SetLayerRecursively(gameObject, normalLayer);
        playerCollider.enabled = true;
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
        }
    } 
   
    // Change Child Layers
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
            
        }
    }
    
    private void OnShadowDivePressed(InputAction.CallbackContext context)
    {
        bool isInLight = IsInLightRange();

        if (isShadowForm)
        {
           ExitShadowForm();
        }
        else
        {
            if (isInLight)
            {
                return;
            }
            EnterShadowForm();
        }
    }
    IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, to);
    }
    }
    

