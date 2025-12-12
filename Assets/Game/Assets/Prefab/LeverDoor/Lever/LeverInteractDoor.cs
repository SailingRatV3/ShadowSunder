using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverInteractDoor : MonoBehaviour
{
    private IDoor door;
    private bool leverUsed = false;
   
   bool leverOpen = false;
  
   public bool triggerEvent = false; // Change this to an event system
   Animator animator;
   [SerializeField] InputActionReference interactAction;
   [SerializeField] private GameObject doorGameObject;
   [SerializeField] private Transform playerTransform;
   [SerializeField] private DetectionZone rangeZone;
   [SerializeField] private GameObject interactionText;
   [SerializeField] private CameraController cameraController;
   [SerializeField] private int doorIndex; 
   [SerializeField] private AudioClip doorOpenSound;
    void Awake()
    {
        leverOpen = false;
        door = doorGameObject.GetComponent<IDoor>();
        
        animator = GetComponent<Animator>();
        interactAction.action.Enable();
        interactionText.SetActive(false);
        interactAction.action.performed += OnInteract;
    }

    void OnDestroy()
    {
        interactAction.action.performed -= OnInteract;
    }
    
   
    private void FixedUpdate()
    {
        if (leverUsed)
        {
            interactionText.SetActive(false);
            return; 
        }

        if (rangeZone.detectedObjects.Count > 0)
        {
            interactionText.SetActive(true);
            leverOpen = true;
        }
        else
        {
            interactionText.SetActive(false);
            leverOpen = false;
        }
       
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (leverUsed) return;         
        if (!leverOpen) return;        

        TriggerLever();
        leverUsed = true;             
        interactionText.SetActive(false);
    }
    

    

    void TriggerLever()
    {
        animator.SetBool("EnableLever", true);
        door.OpenDoor();
        if (triggerEvent)
        {
          //  Debug.Log($"Triggering event for door {doorIndex}");
            SoundFXManager.instance.PlayeSoundFXClip(doorOpenSound, this.transform, 0.4f);
            cameraController.FocusOnDoor(doorIndex);
            StartCoroutine(ReturnCameraToPlayer());
        }
    }
    private IEnumerator ReturnCameraToPlayer()
    {
        
       // Debug.Log("Returning camera to player");
        cameraController.FocusOnPlayer();
        yield return null; 
    }
}
