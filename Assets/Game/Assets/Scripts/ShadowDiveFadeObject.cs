using System.Collections;
using UnityEngine;
using Unity.Collections;
public class ShadowDiveFadeObject : MonoBehaviour
{
    [Header("Sprite Renderer")]
    public SpriteRenderer spriteRenderer;

    [Header("Fade Settings")]
    public float fadedAlpha = 0.5f;
    public float fadeDuration = 0.25f;

    private Coroutine fadeRoutine;
    private float originalAlpha;
    private int originalSortingOrder;

    private void Awake()
    {
        originalAlpha = spriteRenderer.color.a;
        originalSortingOrder = spriteRenderer.sortingOrder;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        ShadowDive dive = other.GetComponentInParent<ShadowDive>();  

        if (dive != null && dive.IsDiving)
        {
            StartFade(fadedAlpha);
            spriteRenderer.sortingOrder += 10;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ShadowDive dive = other.GetComponentInParent<ShadowDive>();  

        if (dive != null)
        {
            StartFade(originalAlpha);
            spriteRenderer.sortingOrder = originalSortingOrder;
        }
    }

    private void StartFade(float targetAlpha)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(targetAlpha));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = spriteRenderer.color.a;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
    }
}
