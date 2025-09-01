using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    public ParticleSystem ps;

    private void OnCollisionEnter(Collision other)
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            ps.Play();
            if (!ps.isPlaying)
            {
              Destroy(this.gameObject);  
            }
            
        }
            
    }
}
