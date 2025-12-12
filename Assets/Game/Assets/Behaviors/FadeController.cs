using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private CanvasGroup canvasGroup; 
    public float fadeDuration = 1.5f;   
    public GameObject endPanel;
   void Awake()
    {
        canvasGroup = endPanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;       
        endPanel.SetActive(false);
    }

    public void StartEndDemoSequence()
    {
        endPanel.SetActive(true);

        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.LockMovement();

        StartCoroutine(FadeInPanel());
    }

    private IEnumerator FadeInPanel()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
