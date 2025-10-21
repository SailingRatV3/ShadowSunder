using System;
using System.Collections;
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

   public RectTransform bossPanelRect;
   public float moveSpeed = 5f;
   private Vector3 originalPosition;
   private Vector3 targetPosition;

   private void Start()
   {
       originalPosition = new Vector3(transform.localPosition.x, -320, transform.localPosition.z);
       targetPosition =  new Vector3(transform.localPosition.x, 20, transform.localPosition.z);
   }

   public void moveBossPanel()
    {
        StopAllCoroutines();  
        StartCoroutine(MovePanelSmoothly(targetPosition));
    }
    public void resetBossPanel()
    {
        StopAllCoroutines();  
        StartCoroutine(MovePanelSmoothly(originalPosition));
    }

    private IEnumerator MovePanelSmoothly(Vector3 target)
    {
        while (Vector3.Distance(bossPanelRect.localPosition, target) > 0.01f)
        {
            bossPanelRect.localPosition = Vector3.Lerp(bossPanelRect.localPosition, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        bossPanelRect.localPosition = target;
    }
   
}
