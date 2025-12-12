using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
   [Header("Cutscene Manager")]
    public SpriteRenderer[] cutsceneImages;
    public TextMeshProUGUI cutsceneText;
    public string[] dialogueLines;
    public float[] imageDisplayTimes;
    public float textDisplayTime = 3f;
    public KeyCode skipKey = KeyCode.Space;
    public CanvasGroup FadeOutCanvasGroup;

    [Header("Key Press Prompt")]
    public SpriteRenderer spacebarImage;
   
    public TextMeshProUGUI spacebarText;
    
    private int currentIndex = 0;
    private CanvasGroup textCanvasGroup;
    private Coroutine spacebarLoopRoutine;
    private bool cutsceneStarted = false;

    void Start()
    {
        FadeOutCanvasGroup = GetComponent<CanvasGroup>();
        textCanvasGroup = cutsceneText.GetComponent<CanvasGroup>();
        spacebarImage.gameObject.SetActive(false);
     
    }

    public void StartCutscene()
    {
        if (cutsceneStarted)
            return; 
        cutsceneStarted = true;
        foreach (var image in cutsceneImages)
            image.gameObject.SetActive(false);

        cutsceneText.text = "";
        textCanvasGroup.alpha = 0;
        currentIndex = 0;
        
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        // Show skip
        yield return StartCoroutine(ShowSkipHintAtStart());

        // Show text lines
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            if (Input.GetKey(skipKey))
            {
                EndCutscene();
                yield break;
            }

            // If different image, fade out and fade in new image
            if (i > 0 && cutsceneImages[i].sprite != cutsceneImages[i - 1].sprite)
            {
                StartCoroutine(FadeImage(cutsceneImages[i - 1], 1f, 0f, 1f));  
                cutsceneImages[i].gameObject.SetActive(true);  
                yield return StartCoroutine(FadeImage(cutsceneImages[i], 0f, 1f, 1f));  
            }
            else if (i == 0)
            {
                
                cutsceneImages[i].gameObject.SetActive(true);
                yield return StartCoroutine(FadeImage(cutsceneImages[i], 0f, 1f, 1f));
            }

            
            cutsceneText.text = dialogueLines[i];
            yield return StartCoroutine(FadeInText(textCanvasGroup, 1f));  

            // Wait for player input 
            yield return StartCoroutine(ShowSpacebarPrompt());
            yield return StartCoroutine(WaitForAnyKeyOrSkip());

            
            yield return StartCoroutine(FadeOutText(textCanvasGroup, 1f));  
        }

        EndCutscene();
    }

    void EndCutscene()
    {
        SaveSystem.ClearCheckpoint();
        SceneManager.LoadScene("DemoLevel_01");
    }

    IEnumerator FadeInText(CanvasGroup canvasGroup, float duration)
    {
        float start = 0f;
        float end = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
       
    }

    IEnumerator FadeOutText(CanvasGroup canvasGroup, float duration)
    {
        float start = 1f;
        float end = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

    IEnumerator ShowSpacebarPrompt()
    {
        spacebarImage.gameObject.SetActive(true);
       
        if (spacebarLoopRoutine != null)
        {
            StopCoroutine(spacebarLoopRoutine);
        }

        spacebarLoopRoutine = StartCoroutine(LoopSpacebarPrompt());

        yield return null;
    }

    IEnumerator LoopSpacebarPrompt()
    {
        float duration = 1f;

        while (spacebarImage.gameObject.activeSelf)
        {
            yield return StartCoroutine(FadeImage(spacebarImage, 0f, 1f, duration));
           
            yield return StartCoroutine(FadeImage(spacebarImage, 1f, 0f, duration));
        }
    }

    IEnumerator FadeImage(SpriteRenderer img, float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, to);
    }
    
    IEnumerator FadeInSpacebar(float duration)
    {
        float start = 0f;
        float end = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            spacebarImage.color = new Color(spacebarImage.color.r, spacebarImage.color.g, spacebarImage.color.b, Mathf.Lerp(start, end, elapsed / duration));
            yield return null;
        }
        spacebarImage.color = new Color(spacebarImage.color.r, spacebarImage.color.g, spacebarImage.color.b, 1f); 

    }
    
    IEnumerator WaitForSpacebarPress()
    {
        
        while (!Input.GetKeyDown(skipKey))
        {
            if (Input.GetKey(skipKey))  
            {
                
                yield break;
            }
            yield return null;
        }
        spacebarImage.gameObject.SetActive(false); 
    }

    IEnumerator WaitForAnyKeyPress()
    {
        while (!Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            yield return null;
        }
        spacebarImage.gameObject.SetActive(false);
    }
    
    IEnumerator ShowSkipHintAtStart()
    {
        spacebarText.gameObject.SetActive(true);
        
        yield return StartCoroutine(FadeTMP(spacebarText, 0f, 1f, 1f));

        
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(FadeTMP(spacebarText, 1f, 0f, 1f));

        spacebarText.gameObject.SetActive(false);
    }
    
    IEnumerator FadeTMP(TextMeshProUGUI text, float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(from, to, elapsed / duration);
            text.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        text.color = new Color(c.r, c.g, c.b, to);
    }
    
    IEnumerator WaitForAnyKeyOrSkip()
    {
        while (true)
        {
            if (Input.GetKey(skipKey))
            {
                EndCutscene();
                yield break;
            }

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                break;

            yield return null;
        }
        if (spacebarLoopRoutine != null)
        {
            StopCoroutine(spacebarLoopRoutine);
            spacebarLoopRoutine = null;
        }

        spacebarImage.gameObject.SetActive(false);
    }
}
