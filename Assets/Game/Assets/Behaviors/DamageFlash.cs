using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // Call this method when hit
    // GetComponent<DamageFlash>().Flash();
    public void Flash()
    {
        if (spriteRenderer != null)
        {
            StopAllCoroutines(); 
            StartCoroutine(FlashRoutine());
        }
    }
    

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}
