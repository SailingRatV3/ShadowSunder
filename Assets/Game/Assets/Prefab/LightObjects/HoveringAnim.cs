using UnityEngine;

public class HoveringAnim : MonoBehaviour
{
    [Header(("Settings"))]
    public float bobbingAmplitudeY = 0.2f;     
    public float bobbingAmplitudeX = 0.05f;    
    public float bobbingFrequency = 1f;        

    private Vector3 initialLocalPosition;
    // adding random oscillation 
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
        // Smooth oscillation
        /*
        float bobY = Mathf.Sin(Time.time * bobbingFrequency * 2 * Mathf.PI) * bobbingAmplitudeY;
        float bobX = Mathf.Cos(Time.time * bobbingFrequency * 2 * Mathf.PI) * bobbingAmplitudeX;

        transform.localPosition = initialLocalPosition + new Vector3(bobX, bobY, 0f);
        */
        transform.localPosition = initialLocalPosition + new Vector3(offsetX, offsetY, 0f);
    }
    /*
     // This Adds a Slight Jitter effect
    public float hoverRadius = 0.2f; // moving from the center
    public float hoverSpeed = 1f; 
    public float directionChangeInterval = 2f; // Change different direction

    private Vector3 targetOffset;
    private Vector3 currentOffset;
    private float timer;

    void Start()
    {
        PickNewTargetOffset();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= directionChangeInterval)
        {
            timer = 0f;
            PickNewTargetOffset();
        }

        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * hoverSpeed);
        transform.localPosition = currentOffset;
    }

    void PickNewTargetOffset()
    {
        float x = Random.Range(-hoverRadius, hoverRadius);
        float y = Random.Range(-hoverRadius, hoverRadius);
        targetOffset = new Vector3(x, y, 0f);
    }
    */
}
