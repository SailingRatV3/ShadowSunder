using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverInteractDoor : MonoBehaviour
{
    private IDoor door;
   // bool canOpen = false;
   bool leverOpen = false;
  [SerializeField] public Teleporter teleporter; // Change this 
   public bool triggerEvent = false; // Change this to an event system
   Animator animator;
   [SerializeField] InputActionReference interactAction;
   [SerializeField] private GameObject doorGameObject;
   [SerializeField] private Transform playerTransform;
   [SerializeField] private DetectionZone rangeZone;
   [SerializeField] private GameObject interactionText;
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
        if (rangeZone.detectedObjects.Count > 0)
        {
            //
            if (interactionText != null)
            {
                interactionText.SetActive(true);
            }
            leverOpen = true;
        } 
        if (rangeZone.detectedObjects.Count == 0)
        {
            interactionText.SetActive(false);
        }
       
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (leverOpen)
        {
            TriggerLever(); 
        }
       
    }
    

    

    void TriggerLever()
    {
        animator.SetBool("EnableLever", true);
        door.OpenDoor();
        if (triggerEvent)
        {
             teleporter.SetActive(true);
        }
    }
}
