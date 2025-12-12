using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleporter : MonoBehaviour
{
     public Transform teleportDestination;  
        public bool isActive = false;        
        private SpriteRenderer spriteRenderer;
        [SerializeField] InputActionReference interactAction;
        [SerializeField] private GameObject interactionText;
        private bool playerInTrigger = false;
        private GameObject player;
       
        private Color originalColor;
        public Color newColor = Color.red;
        
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
        }

        private void OnEnable()
        {
            interactAction.action.performed += OnInteract;
            interactAction.action.Enable();
        }

        private void OnDisable()
        {
            interactAction.action.performed -= OnInteract;
            interactAction.action.Disable();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive) return;

            if (other.CompareTag("Player"))
            {
                playerInTrigger = true;
                player = other.gameObject;
                interactionText?.SetActive(true);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInTrigger = false;
                player = null;
                interactionText?.SetActive(false);
            }
        }
    
        private void OnInteract(InputAction.CallbackContext context)
        {
            if (isActive && playerInTrigger && player != null)
            {
                player.transform.position = teleportDestination.position;
                interactionText?.SetActive(false);
            }
        }
        
        // Call this to activate/deactivate the teleporter
        public void SetActive(bool state)
        {
            isActive = state;
            spriteRenderer.color = newColor;
        }

        private void UpdateVisual()
        {
            if (spriteRenderer != null)
            {
                GetComponent<DamageFlash>().Flash();

            }
        }
        
        
        
        
}
