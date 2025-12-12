using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SaveSystem.SaveCheckPoint(transform.position);
            Debug.Log("Checkpoint saved at: " + transform.position);
        }
    }
    
}
