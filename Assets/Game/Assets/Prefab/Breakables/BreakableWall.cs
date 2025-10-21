using System.Collections;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public void FadeOutAndDisable(SpriteRenderer spriteRenderer, float duration = 0.5f)
    {
        StartCoroutine(FadeOut(spriteRenderer, duration));
    }

    IEnumerator FadeOut(SpriteRenderer spriteRenderer, float duration)
    {
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        spriteRenderer.gameObject.SetActive(false);
    }

}
