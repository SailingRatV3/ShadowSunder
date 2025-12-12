using System.Collections;
using UnityEngine;

public class UIHealthTutorial : MonoBehaviour
{
    public CanvasGroup tutorialPanel;
    public float fadeDuration = 1f;

    private bool tutorialShown = false;
    private bool waitingForInput = false;

    private PlayerInventory playerInventory;

    private void OnEnable()
    {
        PlayerInventory.onInventoryChanged += HandleInventoryChanged;
    }

    private void OnDisable()
    {
        PlayerInventory.onInventoryChanged -= HandleInventoryChanged;
    }

    private void Start()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.alpha = 0f;
            tutorialPanel.blocksRaycasts = false;
            tutorialPanel.interactable = false;
        }

        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    private void HandleInventoryChanged(int newCount)
    {
        // Only trigger when the Player first pickup gem is added
        if (!tutorialShown && newCount == 1)
        {
            tutorialShown = true;
            StartCoroutine(FadeInTutorial());
        }
    }

    IEnumerator FadeInTutorial()
    {
        float t = 0f;

        tutorialPanel.blocksRaycasts = true;
        tutorialPanel.interactable = true;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            tutorialPanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        tutorialPanel.alpha = 1f;
        waitingForInput = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && waitingForInput)
        {
            waitingForInput = false;
            StartCoroutine(FadeOutTutorial());
        }
    }

    IEnumerator FadeOutTutorial()
    {
        float t = 0f;

        tutorialPanel.blocksRaycasts = false;
        tutorialPanel.interactable = false;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            tutorialPanel.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        tutorialPanel.alpha = 0f;
    }
}
