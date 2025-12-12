using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraShakeManager : MonoBehaviour
{
    public CameraShake cameraShake;
    public List<CameraShakePresets> shakePresets;
    public bool triggerCameraShake = false;
    void Start()
    {
        if (cameraShake == null)
        {
            cameraShake = GetComponent<CameraShake>();
        }
    }
    
    void Update()
    {
        if (triggerCameraShake)
        {
            TriggerShakeByName("Hit");
        }
    }
    
    // Trigger a specific Shake Preset by Name
    // ex: cameraShake.TriggerShakeByName(Hit)
    public void TriggerShakeByName(string presetName)
    {
        if (cameraShake == null)
        {
            return;
        }

        if (shakePresets == null || shakePresets.Count == 0)
        {
            return;
        }

        CameraShakePresets preset = shakePresets.Find(p => p.presetName == presetName);
        if (preset != null)
        {
            cameraShake.TriggerShake(preset);
        }
        else
        {
            Debug.LogWarning("Preset Name not found: " + presetName);
        }
    }
    
    // Trigger by Index
    // ex: cameraShake.TriggerShakeByIndex(1)
    public void TriggerShakeByIndex(int index)
    {
        if (index >= 0 && index < shakePresets.Count)
        {
            cameraShake.TriggerShake(shakePresets[index]);
        }
        else
        {
            Debug.LogWarning("Preset Index not found: " + index);
        }
    }
    
}
