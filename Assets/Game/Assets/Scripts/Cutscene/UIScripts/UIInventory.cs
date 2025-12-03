using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(DelayedTextUpdate(count));
    }

    private IEnumerator DelayedTextUpdate(int count)
    {
        yield return null; // wait for next frame
        if (healthPickuptext != null)
            healthPickuptext.text = count.ToString();
    }
    
}
