using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Setup")]
   public Camera mainCamera;
    public Transform[] doorTransforms;    
    public Transform playerTransform;     
    public Transform cameraFollowPoint; 
    
    [Header("Transition Settings")]
    public float transitionSpeed = 2f;    
    public float holdDuration = 1.5f;
    private bool isTransitioning = false;
    private bool isFollowingPlayer = true;
    [SerializeField] private PlayerController playerController;

    void LateUpdate()
    {
        if (isFollowingPlayer && !isTransitioning)
        {
            Vector3 targetPos = new Vector3(playerTransform.position.x, playerTransform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = targetPos;
        }
    }
    public void FocusOnDoor(int doorIndex)
    {
        
        if (isTransitioning || doorIndex >= doorTransforms.Length) return;
      isFollowingPlayer = false; 
        StartCoroutine(FocusDoorSequence(doorTransforms[doorIndex]));
    }

   
    public void FocusOnPlayer()
    {
        if (isTransitioning) return;
        StartCoroutine(SmoothTransitionToTarget(playerTransform, shouldUnlockAfter:true));
        
    }
    
    private IEnumerator FocusDoorSequence(Transform doorTarget)
    {
        playerController.LockMovement();
       
        yield return StartCoroutine(SmoothTransitionToTarget(doorTarget, shouldUnlockAfter:false));

        yield return new WaitForSeconds(holdDuration);

        yield return StartCoroutine(SmoothTransitionToTarget(playerTransform, shouldUnlockAfter:true));
        playerController.UnlockMovement();
        isFollowingPlayer = true;
    }

    private IEnumerator SmoothTransitionToTarget(Transform target, bool shouldUnlockAfter)
    {
        isTransitioning = true;

        Vector3 initialPosition = mainCamera.transform.position;
        float initialZ = initialPosition.z;
        float timeElapsed = 0f;

        
        Vector3 targetPosition = target.position;

        while (timeElapsed < transitionSpeed)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / transitionSpeed);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

         
            Vector3 newPosition = new Vector3(
                Mathf.Lerp(initialPosition.x, targetPosition.x, smoothT),
                Mathf.Lerp(initialPosition.y, targetPosition.y, smoothT),
                initialZ
            );

            mainCamera.transform.position = newPosition;
            yield return null;
        }

        mainCamera.transform.position = new Vector3(target.position.x, target.position.y, initialZ);

        isTransitioning = false;

        if (shouldUnlockAfter)
        {
            playerController.UnlockMovement();
        }
    }
}
