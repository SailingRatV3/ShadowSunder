using UnityEngine;

public class HoveringAnim : MonoBehaviour
{
    [Header(("Settings"))]
    public float bobbingAmplitudeY = 0.2f;     
    public float bobbingAmplitudeX = 0.05f;    
    public float bobbingFrequency = 1f;        

    private Vector3 initialLocalPosition;
    private float noiseSeedX;
    private float noiseSeedY;
    void Start()
    {
        initialLocalPosition = transform.localPosition;
        
        noiseSeedX = Random.Range(0f, 100f);
        noiseSeedY = Random.Range(100f, 200f);
    }

    void Update()
    {
        float time = Time.time * bobbingFrequency;
        
        float noiseX = Mathf.PerlinNoise(noiseSeedX, time) - 0.5f;
        float noiseY = Mathf.PerlinNoise(noiseSeedY, time) - 0.5f;

        float offsetX = noiseX * bobbingAmplitudeX * 2f;
        float offsetY = noiseY * bobbingAmplitudeY * 2f;
        transform.localPosition = initialLocalPosition + new Vector3(offsetX, offsetY, 0f);
    }
    
}
