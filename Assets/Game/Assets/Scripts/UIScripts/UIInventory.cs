using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    // Healing GEM UI
    public TextMeshProUGUI healthPickuptext;

    private void OnEnable()
    {
        PlayerInventory.onInventoryChanged += UpdateUI;
    }

    private void OnDisable()
    {
        PlayerInventory.onInventoryChanged -= UpdateUI;
    }
    private void UpdateUI(int count)
    {
        healthPickuptext.text = count.ToString();
    }
    
}
