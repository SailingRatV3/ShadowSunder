using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class TutorialTrigger : MonoBehaviour
{
    [Header("Tutorial Trigger")]
    public CanvasGroup tutorialPanel;

    private bool playerInside = false;
    private bool tutorialShown = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!tutorialShown && other.CompareTag("Player"))
        {
            playerInside = true;
            StartCoroutine(FadeInPanel());
        }
    }

    void Update()
    {
        if (playerInside && tutorialPanel.alpha == 1f && Input.anyKeyDown)
        {
            StartCoroutine(FadeOutPanel());
        }
    }

    IEnumerator FadeInPanel()
    {
        tutorialPanel.blocksRaycasts = true;
        tutorialPanel.interactable = true;

        float duration = 1f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            tutorialPanel.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }

        tutorialPanel.alpha = 1f;
    }

    IEnumerator FadeOutPanel()
    {
        tutorialPanel.blocksRaycasts = false;
        tutorialPanel.interactable = false;

        float duration = 1f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            tutorialPanel.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }

        tutorialPanel.alpha = 0f;
        tutorialShown = true;
        playerInside = false;
    }
}
