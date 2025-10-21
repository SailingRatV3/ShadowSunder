using UnityEngine;

public class InnerBarrier : MonoBehaviour
{
    private RingBarrier parentBarrier;

    void Start()
    {
        parentBarrier = transform.parent.GetComponent<RingBarrier>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        parentBarrier.RegisterInner(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        parentBarrier.UnregisterInner(other);
    }
}
