using UnityEngine;
[System.Serializable]
public class CameraShakePresets
{
    // This is a stored Customizable Preset for Camera Shake 
    public string presetName;
    public AnimationCurve curve;
    public float duration = 1f;
    public float shakeStrength = 1f;
    public float shakeFrequency = 10f; 
}
