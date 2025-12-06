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
    public GameObject mainMenuPanel; // main menu panel 
    public GameObject cutsceneBackground; // Reference to the cutscene background image
    public Button playButton; // Reference to the Play button
    public Animator mainMenuAnimator; // Reference to the main menu animator

    public string cutsceneSceneName = "CutsceneScene"; 
    public float transitionDuration = 1f; // Duration of the transition

    private CutsceneManager cutsceneManager;
    
    void Start()
    {
        
        cutsceneManager = FindObjectOfType<CutsceneManager>();

        
        playButton.onClick.AddListener(OnPlayButtonPressed);
        
        
      //  cutsceneBackground.SetActive(false);
    }

    void OnPlayButtonPressed()
    {
        // Start the transition out for the main menu and background move-in
        StartCoroutine(PlayCutsceneTransition());
    }

    IEnumerator PlayCutsceneTransition()
    {
        // Play the main menu exit animation (move off-screen)
        mainMenuAnimator.SetTrigger("ExitMenu"); 
        
        // Wait for the transition duration (same time as the menu animation)
        yield return new WaitForSeconds(transitionDuration);

        // Optionally, enable and play the cutscene background enter animation if needed
        // cutsceneBackground.SetActive(true); // Show the cutscene background
        // cutscene background
        //cutsceneBackgroundAnimator.SetTrigger("EnterBackground");

        // Wait for the cutscene background animation to complete (if applicable)
        yield return new WaitForSeconds(transitionDuration);

        // Now trigger the CutsceneManager to start the cutscene
        if (cutsceneManager != null)
        {
            cutsceneManager.StartCutscene(); // Assuming you have a StartCutscene method
        }

        // Optionally, you can load the cutscene scene directly if the CutsceneManager is in a different scene
        //SceneManager.LoadScene(cutsceneSceneName);
    }
    
}
