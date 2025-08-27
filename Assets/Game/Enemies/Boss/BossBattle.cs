using UnityEngine;

public class BossBattle : MonoBehaviour
{
    [SerializeField] private ColliderTrigger colliderTrigger;
    [SerializeField] private StormMageBoss stormMageBoss;
    // Enemy Spawning 
    // Note: Write EnemySpawn Script
   // [SerializeField] private EnemySpawn enemySpawnPrefab;
   // private List<Vector3> spawnLocationList // Have a list of spawn location to spawn enemies
//https://www.google.com/search?client=firefox-b-1-d&sca_esv=49d605c33f00d0c3&q=how+to+make+a+boss+in+unity+2d+top+down&source=lnms&fbs=AIIjpHz30rPMyW-0vSP0k1VTNmO_kCOARpjPjQRkBWH2HwUIz5XUSIJvSK0oms7XOxizDlnN-OPyazVTEOWCGKzKDScJu8vbYB83624YbgHUyEfg5sBceZCGD0K6kTp41i6UKLt0F5fd3cMMiXYB4NND7pmo3btwxp4Un-ARvOTT9QrSgV1SnbgYq__EsMkrWgWGx6TO4nIQcI4XOQXbnBWZ_M5TE26GaOIUwxzeqjArL8PJ8fhSVcw&sa=X&ved=2ahUKEwiQgubb86iPAxXSnCYFHd-UNdkQ0pQJegQICxAB&biw=1536&bih=779&dpr=1.25#fpstate=ive&vld=cid:548b1a43,vid:qZC1VYWnHZ8,st:0
// 10:46
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
    
    private void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        StartBattle();
        colliderTrigger.OnPlayerTriggerEnter -= ColliderTrigger_OnPlayerEnterTrigger;
        
    }

    private void StartBattle()
    {
        Debug.Log("StartBattle");
        // SpawnEnemy
        // Note: Have when the boss spawn enemy attack spawn enemies
    }

    private void SpawnEnemy()
    {
        // Vector3 spawnPosition = spawnLocationList[Random.Range(0, spawnLocationList.Count)];
       // EnemySpawn enemySpawn = Instantiate(enemySpawnPrefab, spawnPosition, Quaternion.identity); // Spawn Enemy
       // enemySpawn.Spawn(); // Call Spawn Function
    }
}
