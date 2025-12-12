using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseLevel : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] InputActionReference usePauseAction;
    [SerializeField] private GameObject pauseMenuPanel;
    
    private bool paused = false;
    
    void Start()
    {
       // Debug.Log("Pause Menu Active: " + pauseMenuPanel.activeSelf);
      //  Debug.Log("Time Scale: " + Time.timeScale);
       
    }
    private void OnEnable()
    {
        usePauseAction.action.Enable();
        usePauseAction.action.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        usePauseAction.action.performed -= OnPausePerformed;
        usePauseAction.action.Disable();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        
        TogglePause();
    }
    
    private void TogglePause()
    {
        if (paused)
        {
            
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
      //  Debug.Log("Resume Button Clicked");
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;     
        paused = false;
        
        
    }
    
    void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
    
}
