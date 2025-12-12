using System;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    public event EventHandler OnBossStarted;
    public event EventHandler OnBossEnded;
    public enum Stage
    {
        WaitingToStart,
        Stage_1,
        Stage_2,
        Stage_3,
    }
    [SerializeField] private ColliderTrigger colliderTrigger;
    [SerializeField] private StormMageBoss stormMageBoss;
    [SerializeField] private Transform healthPickup;
    private Stage stage;
    
    // Enemy Spawning 
    // Note: Write EnemySpawn Script
   // [SerializeField] private EnemySpawn enemySpawnPrefab;
   private List<Vector3> spawnLocationList; // Have a list of spawn location to spawn enemies
   // private List<EnemySpawn> enemySpawnList;
   private void Awake()
   {
       /*
        * spawnPositionList = new List<Vector3>();
        * foreach(Transform spawnPosition in transform.Find("spawnPositions"))
        * {
        *   spawnLocationList.Add(spawnPosition.position);
        * }
        */
       
   }
    private void Start()
    {
        colliderTrigger.OnPlayerTriggerEnter += ColliderTrigger_OnPlayerEnterTrigger;
    }

    private void BossBattle_OnDeath(object sender, System.EventArgs e)
    {
        Debug.Log("Boss Battle Ended");
        DestroyAllEnemies();
        
        OnBossEnded?.Invoke(this, EventArgs.Empty);
    }

    private void BossBattle_OnDamage(object sender, System.EventArgs e)
    {
        // When Boss take DMG
        switch (stage)
        {
            //default: 
                
           case Stage.Stage_1:
               StartNextStage();
                //if()
                break;
            case Stage.Stage_2:
                StartNextStage();
                //if()
                break;
            case Stage.Stage_3:
                StartNextStage();
                //if()
                break;
        }
        
    }
    
    private void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        StartBattle();
        colliderTrigger.OnPlayerTriggerEnter -= ColliderTrigger_OnPlayerEnterTrigger;
        
    }

    private void StartBattle()
    {
        // Debug.Log("StartBattle");
        stormMageBoss.StartBossPhase();
        StartNextStage();
       
        OnBossStarted?.Invoke(this, EventArgs.Empty);
    }

    private void StartNextStage()
    {
        switch (stage)
        {
           
                case Stage.WaitingToStart:
                    stage = Stage.Stage_1;
                    break;
            case Stage.Stage_1:
                stage = Stage.Stage_2;
                
                break;
            case Stage.Stage_2:
                stage = Stage.Stage_3;
                break;
            
                
        }
      
    }
    private void SpawnEnemy()
    {
        // int aliveCount = 0;
        // foreach (EnemySpawn enemySpawned in enemySpawnList){
        // if(enemySpawned.IsAlive()){
        // Enemy Alive
        // aliveCount++;
        // if(aliveCount >=3){
        // return; 
        //}
        //}
        // }
        // Vector3 spawnPosition = spawnLocationList[Random.Range(0, spawnLocationList.Count)];
       // EnemySpawn enemySpawn = Instantiate(enemySpawnPrefab, spawnPosition, Quaternion.identity);
       // enemySpawn.Spawn(); 
       
       // enemySpawnList.Add(enemySpawn);
    }

    private void DestroyAllEnemies()
    {
        Debug.Log("DestroyAllEnemies");
        // foreach(EnemySpawn enemySpawn in enemySpawnList){
            /*
             * if(enemySpawn.IsAlive()){
             *  enemySpawn.KillEnemy();
             * }
             */
    //}

}

    private void SpawnHealthPickup()
    {
       // Vector3 spawnPosition = spawnPositionList[Random.Range(0, spawnPositionList.Count)];
        // Transform healthPickupTransform = Instantiate(healthPickup, spawnPosition, Quaternion.identity);
    }
    
}
