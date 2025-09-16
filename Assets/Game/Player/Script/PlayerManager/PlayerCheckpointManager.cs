using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCheckpointManager : MonoBehaviour
{
    private Vector3 CheckpointPosition;
    private Vector3 startingPosition;
    void Start()
    {
        startingPosition = transform.position;
        CheckpointPosition = startingPosition;
    }

    void Update()
    {
        // Press R to Reset -- change this before final Demo Test
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetToCheckpoint();
        }
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        CheckpointPosition = newCheckpoint;
    }
    public void ResetToCheckpoint()
    {
        transform.position = CheckpointPosition;
        Debug.Log("Player reset to Checkpoint at:" + CheckpointPosition);
    }
    
    // Call this on Player Death (playerCheckpointManager.OnPlayerDeath();
    public void OnPlayerDeath()
    {
        ResetToCheckpoint();
    }
    
}
