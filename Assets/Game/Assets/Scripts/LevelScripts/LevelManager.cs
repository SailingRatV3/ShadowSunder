using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    [Header("Button")]
    public Button resumeButton;
    public Button restartButton;
    public Button settingsButton;
    public Button quitButton;
    [Header("Pause Settings")]
    [SerializeField] InputActionReference usePauseAction;
    [SerializeField] private GameObject pauseMenuPanel;
    
    private bool paused = false;
    
    void Start()
    {
        if (SaveSystem.HasCheckpoint())
        {
            transform.position = SaveSystem.LoadCheckpoint();
        }
        
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
        if (pauseMenuPanel == null)
        {
            return;
        }
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;     
        paused = false;
        
        
    }
    
    void Pause()
    {
        if (pauseMenuPanel == null)
        {
            return;
        }
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
    
    [Header("New Game")]
    public bool isNewGame = false;
    
    int newGameCount = 0;
    
    [Header("restart")]
   [SerializeField] private PlayerRespawnManager playerRespawnManager;
    
    void Awake()
    {
        Time.timeScale = 1f;
    }
    public void ResumeGame()
    {
        Resume();
        pauseMenuPanel.SetActive(false);
    }

    public void RestartGame()
    {
        playerRespawnManager.ReloadSceneToCheckpoint();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void EndGame()
    {
        
    }

    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(AssignButtonsNextFrame());
    }

    private IEnumerator AssignButtonsNextFrame()
    {
        yield return null; // wait one frame

        pauseMenuPanel = GameObject.Find("PauseMenuPanel");

        resumeButton = GameObject.Find("ResumeLevel_Btn")?.GetComponent<Button>();
        restartButton = GameObject.Find("RestartLevel_Btn")?.GetComponent<Button>();
        settingsButton = GameObject.Find("Settings_Btn")?.GetComponent<Button>();
        quitButton = GameObject.Find("QuitMainMenu_Btn")?.GetComponent<Button>();

        Debug.Log("resumeButton found: " + (resumeButton != null));

        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(Resume);
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(OpenSettings);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(ReturnToMainMenu);
        }
    }
    
}
