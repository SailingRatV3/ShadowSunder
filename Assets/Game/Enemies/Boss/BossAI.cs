using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossAI : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public Transform centerPoint;
    public float moveSpeed = 3.5f;
    public float attackDelay = 2f;
    public int damageThreshold = 10;
    public float pushForce = 10f;
    
    private int currentPointIndex = 0;
    private float recievedDamage = 0;
    private bool isAttacking = false;
    private bool isResetting = false;
    
    private Transform target;
    private NavMeshAgent agent;
    
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        goToNextPoint();
    }
    
    // NOTE: Maybe change this with the take damage
    public void TakeDamage(float amount)
    {
        recievedDamage += amount;
        if (!isResetting && recievedDamage >= damageThreshold)
        {
            StartCoroutine(ResetToCenter());
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= 0.2f && !isAttacking && !isResetting)
        {
            StartCoroutine(AttackAtPoint());
        }
    }

    IEnumerator AttackAtPoint()
    {
        // Simulate Attack 
        Debug.Log("Boss Attacking at point " + currentPointIndex);
        yield return new WaitForSeconds(attackDelay);
        
        isAttacking = false;
        
        // Move to next point
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
        goToNextPoint();
    }
    
    public void goToNextPoint()
    {
        if (patrolPoints.Count == 0)
        {
            return;
        }
        
        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    IEnumerator ResetToCenter()
    {
        isResetting = true;
        isAttacking = false;
        
        // push back player
        Vector2 pushDir = (player.position - transform.position).normalized;
        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
        }
        
        Debug.Log("Resetting to center");
        agent.SetDestination(centerPoint.position);
        
        // wait until boss moves to center
        while (Vector2.Distance(transform.position, centerPoint.position) > 0.2f)
        {
            yield return null;
        }
        
        recievedDamage = 0;
        currentPointIndex = 0;
        
        yield return new WaitForSeconds(1f); // pausing at the center
        
        isResetting = false;
        
        goToNextPoint();
        
    }
    
}
/*

// NOTE: if using the take damage add to player:
void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Boss"))
    {
        BossAI boss = other.GetComponent<BossAI>();
        if (boss != null)
        {
            boss.TakeDamage(5f);
        }
    }
}

*/