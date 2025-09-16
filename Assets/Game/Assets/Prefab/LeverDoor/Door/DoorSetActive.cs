using UnityEngine;

public class DoorSetActive : MonoBehaviour, IDoor
{
    private bool isOpen = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OpenDoor()
    {
        gameObject.SetActive(false);
      // animator.SetBool("OpenDoor", true);
    }

    public void CloseDoor()
    {
        gameObject.SetActive(true);
     // animator.SetBool("OpenDoor", false);
    }

    public void PlayOpenFailAnim()
    {
        // animator.SetTrigger("OpenFail");
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
