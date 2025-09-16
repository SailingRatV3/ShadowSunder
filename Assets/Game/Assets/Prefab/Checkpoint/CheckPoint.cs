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
    
 /*   
    public Collider2D checkpointRange;
    Animator animator;
    bool checkPointActive = false;
    Vector2 checkPointPosition;
    Vector2 playerOldSavedPosition; // This is not necessary
    Vector2 playerNewSavedPosition;
    List checkPointsList = new List(); 
    void Start()
    {
        animator = GetComponent<Animator>();
        checkPointPosition = this.transform.position;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isCheck", true);
            checkPointActive = true;
        }
    }

    void SaveLastPosition()
    {
        if (checkPointActive)
        {
            playerNewSavedPosition = checkPointPosition;
            
            if (playerOldSavedPosition != null)
            {
               playerOldSavedPosition = playerNewSavedPosition; 
            }
        }
        
    }
// Add this to player object (calculates closest Checkpoints)
// 
    Transform GetClosestCheckPoint(Transform[] checkPoints)
    {
        Transform closestCheckPoint = null;
        float closestDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;
        foreach (Transform t in checkPoints)
        {
            float dis = Vector2.Distance(t.position, currentPosition);
            if (dis < closestDistance)
            {
                closestCheckPoint = t;
                closestDistance = dis;
            }
        }
        return closestCheckPoint;
    }
    
    void ReloadToLastPosition()
    {
        if (checkPointActive)
        {
            
        }
    }
    */
}
