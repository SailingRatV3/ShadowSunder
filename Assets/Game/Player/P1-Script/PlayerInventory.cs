using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    
    // Health Gem Inventory Var
    [Header("Heal Inventory Settings")] 
    public int healthPickupCount = 0;
    public float healAmount = 1f;
    [SerializeField] InputActionReference useHealAction;
    
    private PlayerHealth playerHealth;
    
    public delegate void OnInventoryChanged(int newCount);
    public static event OnInventoryChanged onInventoryChanged;


    private void Awake()
    {
        useHealAction.action.performed += OnHeal;

    }

    void OnDestroy()
    {
        useHealAction.action.performed -= OnHeal;
    }
    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
      
    }

    public void AddHealthPickup()
    {
        healthPickupCount++;
        onInventoryChanged?.Invoke(healthPickupCount);
    }

    public void UseHealthPickup()
    {
        if (healthPickupCount > 0 && playerHealth != null && !playerHealth.IsFullHealth)
        {
            playerHealth.Heal(healAmount);
            healthPickupCount--;
            onInventoryChanged?.Invoke(healthPickupCount);
        }
        
    }
    
    private void OnHeal(InputAction.CallbackContext context)
    {
        UseHealthPickup();
    }
}
