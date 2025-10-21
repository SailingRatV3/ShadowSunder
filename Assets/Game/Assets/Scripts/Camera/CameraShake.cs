using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header(("Shake Settings"))]
    public bool start = false;
    private Vector3 originalPosition;
    private bool isShaking = false;
    
    public CameraShakePresets currentPreset;
    
    

    IEnumerator Shaking(CameraShakePresets preset)
    {
        isShaking = true;
        originalPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < preset.duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = preset.curve.Evaluate(elapsedTime / preset.duration)*preset.shakeStrength;
            // Smoothing randomness
            float xShake = Mathf.PerlinNoise(Time.time * preset.shakeFrequency, 0f) * 2f - 1f;
            float yShake = Mathf.PerlinNoise(0f, Time.time * preset.shakeFrequency) * 2f - 1f;
            
            transform.localPosition = originalPosition + new Vector3(xShake, yShake, 0f) * strength;            yield return null;
        }
        transform.localPosition = originalPosition;        
        isShaking = false;
    }
    
    public void TriggerShake(CameraShakePresets preset)
    {
        if (!isShaking)
            StartCoroutine(Shaking(preset));
    }
}
