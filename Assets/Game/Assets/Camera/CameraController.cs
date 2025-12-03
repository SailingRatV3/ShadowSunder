using System.Collections;
using UnityEngine;
//using Unity.Cinemachine;
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
        // playerController.LockMovement();   
        if (isTransitioning || doorIndex >= doorTransforms.Length) return;
       // playerController.LockMovement();
       // Debug.Log($"Focusing on door {doorIndex}");
      //  Debug.Log("Target door position: " + doorTransforms[doorIndex].position);
      isFollowingPlayer = false; 
        StartCoroutine(FocusDoorSequence(doorTransforms[doorIndex]));
    }

    // Switch back to player camera 
    public void FocusOnPlayer()
    {
        if (isTransitioning) return;
        Debug.Log("Focusing on player");
       // playerController.UnlockMovement(); 
        StartCoroutine(SmoothTransitionToTarget(playerTransform, shouldUnlockAfter:true));
        
    }
    
    private IEnumerator FocusDoorSequence(Transform doorTarget)
    {
        playerController.LockMovement();
        // Move camera to the door
        yield return StartCoroutine(SmoothTransitionToTarget(doorTarget, shouldUnlockAfter:false));

        Debug.Log($"[Camera] Holding on door for {holdDuration} seconds...");
        yield return new WaitForSeconds(holdDuration);

        // Move camera back to player and unlock movement
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

            // Smoothly interpolate the position
            Vector3 newPosition = new Vector3(
                Mathf.Lerp(initialPosition.x, targetPosition.x, smoothT),
                Mathf.Lerp(initialPosition.y, targetPosition.y, smoothT),
                initialZ
            );

            mainCamera.transform.position = newPosition;
            yield return null;
        }

        // Ensure the camera is exactly at the target position at the end of the transition
        mainCamera.transform.position = new Vector3(target.position.x, target.position.y, initialZ);

        isTransitioning = false;

        if (shouldUnlockAfter)
        {
            Debug.Log("[Camera] Transition complete — unlocking player movement");
            playerController.UnlockMovement();
        }
    }
}
