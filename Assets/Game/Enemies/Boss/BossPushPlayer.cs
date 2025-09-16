using UnityEngine;

public class BossPushPlayer : MonoBehaviour
{
    Rigidbody2D rb;


    void PushPlayer(Vector2 knockback)
    {
        rb.AddForce(knockback, ForceMode2D.Impulse);
    }
    
}
