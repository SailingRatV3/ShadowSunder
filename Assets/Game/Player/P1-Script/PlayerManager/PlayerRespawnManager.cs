using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
// using DG.Tweening;
public class PlayerRespawnManager : MonoBehaviour
{
    [SerializeField] private GameObject reloadMenu;
    public UIManager uiManager;
    
    void Start()
    {
        if (SaveSystem.HasCheckpoint())
        {
            transform.position = SaveSystem.LoadCheckpoint();
        }
        else
        {
            NewGame();
        }
    }

    void Update()
    {
        // Loads player if restarted
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ReloadSceneToCheckpoint();
        }   
    }

    public void ReloadSceneToCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    
    // Call this on Player Death 
    public void OnPlayerDeath()
    {
       // Debug.Log("Player Death");
      
        uiManager.reloadPanelAnim();
      
        ReloadSceneToCheckpoint();
    }
    // Clear Saved Checkpoint in Starting a new Game
    public static void NewGame()
    {
        SaveSystem.ClearCheckpoint();
    }

    public void StartNewGame()
    {
        SaveSystem.ClearCheckpoint();
        SceneManager.LoadScene("DemoLevel_01");
    }
    
    
}
