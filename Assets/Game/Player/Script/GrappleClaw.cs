using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleClaw : MonoBehaviour
{
     LineRenderer line;
     [Header("References")]
     [SerializeField] Transform lineOrigin;
     [SerializeField] InputActionReference grappleAction;
     [Header("Settings")]
     [SerializeField] LayerMask grappleMask;
     [SerializeField] float maxDistance;
     [SerializeField] float grappleSpeed;
     [SerializeField] float grappleShootSpeed;
     
     bool isGrapple = false;
     [HideInInspector] public bool retracting = false;

      Vector2 target;
      Transform pulledTarget;

      private void Start()
      {
          line = GetComponent<LineRenderer>();
          line.enabled = false;
          line.useWorldSpace = true;

          grappleAction.action.Enable();
          
          grappleAction.action.started += ctx => StartGrapple();
          grappleAction.action.canceled += ctx => OnGrappleEnded();

      }

      private void OnDestroy()
      {
          grappleAction.action.started -= ctx => StartGrapple();
          grappleAction.action.canceled -= ctx => OnGrappleEnded();
      }

      void Update()
      {
          if (isGrapple && pulledTarget != null)
                    {
                        //  floaty : lerp
                        // physichal : moveTowards
                        Vector2 pullTarget = lineOrigin.position;
                        pulledTarget.position = Vector2.MoveTowards(pulledTarget.position, pullTarget, grappleSpeed * Time.deltaTime);
                        
                        // Vector2 grapplePos = Vector2.MoveTowards(transform.position, target,  grappleSpeed * Time.deltaTime);
                       // transform.position = grapplePos;
                       line.enabled = true;
                        line.SetPosition(0, lineOrigin.position);
                        line.SetPosition(1, pulledTarget.position);
                       // line.SetPosition(1, transform.position);
                       /* if (Vector2.Distance(pulledTarget.position, pullTarget) < 0.5f)
                        {
                            retracting = false;
                          //  isGrapple = false;
                           //  line.enabled = false;
                        }*/
                    }

          if (isGrapple && pulledTarget != null)
          {
              line.enabled = true;
              line.SetPosition(0, lineOrigin.position);
              line.SetPosition(1,pulledTarget.position);
              // line.SetPosition(1, target);
          }
      }

      // Right Click
      void OnGrapple()
      {
          Debug.Log("Grapple Function Called");
          if(!isGrapple && !retracting)
          {
              Debug.Log("not grapple");
             StartGrapple(); 
          }

          
          
          
          
      }

      private void StartGrapple()
      {
          if (isGrapple)
              return;
          
          // Debug.Log("Start Grapple Function Called");
         // Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
          
         
         Vector3 mouseWorldPos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);
         Vector2 direction = (mouseWorldPos2D - (Vector2)lineOrigin.position).normalized;
         
         RaycastHit2D hit = Physics2D.Raycast(lineOrigin.position, direction, maxDistance, grappleMask);
        // Debug.DrawLine(lineOrigin.position, target, Color.red, 2f);
         if (hit.collider != null)
          {
              //Debug.Log(hit.collider.gameObject.name);
            //  Debug.Log("isGrapple = true");
            pulledTarget = hit.collider.transform;
            target = pulledTarget.position;
            
              isGrapple = true;
             // retracting = false;
              target = hit.point;
              line.enabled = true;
              line.positionCount = 2;
              
              StartCoroutine(GrappleTime());
          }
      }

      void OnGrappleEnded()
      {
          isGrapple = false;
         // retracting = false;
          line.enabled = false;
          pulledTarget = null;
      }
      IEnumerator GrappleTime()
      {
          if(pulledTarget == null)
              yield break;
          
          float t = 0;
          float duration = 0.3f; // how long to animate line shoot
          Vector2 start = lineOrigin.position;
          Vector2 end = target;
          // Freeze claw while line is shooting
          // transform.position = lineOrigin.position;

          while (t < duration)
          {
              t += Time.deltaTime * grappleShootSpeed;
              Vector2 point = Vector2.Lerp(start, end, t / duration);

              line.SetPosition(0, lineOrigin.position);
              line.SetPosition(1, point);
              yield return null;
          }

          // Finalize position
          line.SetPosition(1, pulledTarget.position);
         // transform.position = lineOrigin.position;

          isGrapple = true; // now the claw moves
      }
}
