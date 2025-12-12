using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        
    }

    public void Credits()
    {
        
    }
    
    [Header("Main Menu Animation")]
    public GameObject mainMenuPanel; 
    public GameObject cutsceneBackground; 
    public Button playButton; 
    public Animator mainMenuAnimator; 

    public string cutsceneSceneName = "CutsceneScene"; 
    public float transitionDuration = 1f; 

    private CutsceneManager cutsceneManager;
    
    void Start()
    {
        
        cutsceneManager = FindObjectOfType<CutsceneManager>();

        
        playButton.onClick.AddListener(OnPlayButtonPressed);
       
    }

    void OnPlayButtonPressed()
    {
        StartCoroutine(PlayCutsceneTransition());
    }

    IEnumerator PlayCutsceneTransition()
    {
        mainMenuAnimator.SetTrigger("ExitMenu"); 
        
        yield return new WaitForSeconds(transitionDuration);

        yield return new WaitForSeconds(transitionDuration);

        if (cutsceneManager != null)
        {
            cutsceneManager.StartCutscene(); 
        }

    }
    
}
