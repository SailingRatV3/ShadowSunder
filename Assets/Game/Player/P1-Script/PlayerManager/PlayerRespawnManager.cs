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
        //  uiManager = GetComponent<UIManager>();
//        reloadMenu.SetActive(false);
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
       // reloadPanelAnim();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    
    // Call this on Player Death (playerRespawnManager.OnPlayerDeath();
    public void OnPlayerDeath()
    {
        Debug.Log("Player Death");
        // reloadMenu.SetActive(true);
        uiManager.reloadPanelAnim();
       // UIManager.reloadPanelAnim();
        ReloadSceneToCheckpoint();
    }
    // Clear Saved Checkpoint in Starting a new Game
    public static void NewGame()
    {
        SaveSystem.ClearCheckpoint();
       // SceneManager.LoadScene("DemoLevel_01");
    }

    public void StartNewGame()
    {
        SaveSystem.ClearCheckpoint();
        SceneManager.LoadScene("DemoLevel_01");
    }
    
    
}
