using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Text Color")]
    public TextMeshProUGUI btnText;
    public Color normalTxtColor;
    public Color hoverTxtColor;
    
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change color
        btnText.color = hoverTxtColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert Colors
        btnText.color = normalTxtColor;
    }
}
