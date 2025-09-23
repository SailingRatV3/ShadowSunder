using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void SaveCheckPoint(Vector3 position)
    {
        PlayerPrefs.SetFloat("CheckpointX", position.x);
        PlayerPrefs.SetFloat("CheckpointY", position.y);
        PlayerPrefs.SetFloat("CheckpointZ", position.z);
        PlayerPrefs.Save();
    }

    public static Vector3 LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            return new Vector3(x,y,z);
        }
        // spawn if player didn't reach to checkpoint
        return Vector3.zero;
    }

    public static bool HasCheckpoint()
    {
        return PlayerPrefs.HasKey("CheckpointX");
    }

    public static void ClearCheckpoint()
    {
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
    }
}
