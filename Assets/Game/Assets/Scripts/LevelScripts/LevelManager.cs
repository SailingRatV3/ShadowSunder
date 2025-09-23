using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool isNewGame = false;
    
    int newGameCount = 0;
    private void Start()
    {
        if (isNewGame == true && newGameCount == 0)
        {
            PlayerRespawnManager.NewGame();
            
        }
    }
}
