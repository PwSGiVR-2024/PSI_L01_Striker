using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public enum AIState { Idle, Search, Chase }
    public AIState currentState = AIState.Idle;

    private NavMeshAgent agent;
    private DetectionSystem detectionSystem;
    private Vector3 lastKnownPosition;

    public Transform[] patrolPoints;
    private int patrolIndex;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        detectionSystem = GetComponent<DetectionSystem>();
        patrolIndex = 0;
    }

    private void Update()
    {
        UpdateState();

        switch (currentState)
        {
            case AIState.Idle:
                Patrol();
                break;
            case AIState.Search:
                Search();
                break;
            case AIState.Chase:
                Chase();
                break;
        }
    }

    private void UpdateState()
    {
        if (detectionSystem.CanSeePlayer())
        {
            currentState = AIState.Chase;
            lastKnownPosition = detectionSystem.GetPlayerPosition();
        }
        else if (detectionSystem.CanHearPlayer())
        {
            currentState = AIState.Search;
            lastKnownPosition = detectionSystem.GetSoundPosition();
        }
        else if (currentState != AIState.Idle)
        {
            currentState = AIState.Idle;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    private void Search()
    {
        agent.SetDestination(lastKnownPosition);
        if (Vector3.Distance(transform.position, lastKnownPosition) < 1.0f)
        {
            currentState = AIState.Idle;
        }
    }

    private void Chase()
    {
        agent.SetDestination(detectionSystem.GetPlayerPosition());
    }
}
