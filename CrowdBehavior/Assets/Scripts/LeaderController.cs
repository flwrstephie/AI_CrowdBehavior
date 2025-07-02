using UnityEngine;
using UnityEngine.AI;

public class LeaderController : MonoBehaviour
{
    private NavMeshAgent agent;

    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float minWanderDistance = 2f;

    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            if (!agent.pathPending && agent.remainingDistance <= 0.5f)
            {
                Vector3 newPos = GetValidWanderPoint();
                agent.SetDestination(newPos);
                timer = 0f;
            }
        }
    }

    Vector3 GetValidWanderPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randDirection = Random.insideUnitSphere * wanderRadius;
            randDirection += transform.position;

            if (NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, wanderRadius, NavMesh.AllAreas))
            {
                float distance = Vector3.Distance(transform.position, navHit.position);
                if (distance >= minWanderDistance)
                    return navHit.position;
            }
        }

        return transform.position;
    }
}
