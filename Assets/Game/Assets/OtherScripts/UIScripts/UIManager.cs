using UnityEngine;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
  //  [SerializeField] private GameObject reloadMenu;
    [SerializeField] private RectTransform reloadPanelRect;
    [SerializeField] float topPosY, bottomPosY;
    [SerializeField] float topPosX, bottomPosX;
    [SerializeField] float tweenDuration;

    public void reloadPanelAnim()
    {
        reloadPanelRect.DOAnchorPosY(bottomPosY, tweenDuration).SetUpdate(true);
        reloadPanelRect.DOAnchorPosX(bottomPosX, tweenDuration).SetUpdate(true);
    }
    
   public void resetReloadPanel()
    {
        reloadPanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true);
        reloadPanelRect.DOAnchorPosX(topPosX, tweenDuration).SetUpdate(true);
    }
}
