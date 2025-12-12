using UnityEngine;

public class DoorSetActive : MonoBehaviour, IDoor
{
    private bool isOpen = false;
    private Animator animator;
    public BreakableWall breakableWall;
    public SpriteRenderer roomShade;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OpenDoor()
    {
        gameObject.SetActive(false);
        breakableWall.FadeOutAndDisable(roomShade, 0.5f);
     
    }

    public void CloseDoor()
    {
        gameObject.SetActive(true);
    }

    public void PlayOpenFailAnim()
    {
     
    }
    
    public void ToggleDoor()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
}
