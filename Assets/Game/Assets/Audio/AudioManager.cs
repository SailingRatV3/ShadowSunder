using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public static AudioManager instance;
    AudioSource audioSource;
    
    [Header ("Collectible Sound")]
    [SerializeField] AudioClip healthSound, pickupSound;

    private void OnEnable()
    {
        HealthPickup.OnHealthCollected += PlayHealthSound;
    }

    private void OnDisable()
    {
        HealthPickup.OnHealthCollected -= PlayHealthSound;
    }
    private void Awake()
    {
        
    }

    public void PlaySound(AudioClip clip)
    {
        
    }

    public void PlayHealthSound()
    {
        // health sound
        // PlayAudioClip(healthSound);
        audioSource.PlayOneShot(healthSound);
        
    }
}
