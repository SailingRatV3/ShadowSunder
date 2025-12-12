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
        btnText.color = hoverTxtColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btnText.color = normalTxtColor;
    }
}
