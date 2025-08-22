using UnityEngine;
using System.Collections.Generic;
public class DetectionZone : MonoBehaviour
{
    public string tagTarget = "Player";
    public List<Collider2D> detectedObjects = new List<Collider2D>();
    public Collider2D col;

   

    // Detects Enter Obj
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == tagTarget)
        {
            detectedObjects.Add(collider);
        }
        
    }
    
    // Detects Exit Obj
    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == tagTarget)
        {
            detectedObjects.Remove(collider);
        }
    }
}
