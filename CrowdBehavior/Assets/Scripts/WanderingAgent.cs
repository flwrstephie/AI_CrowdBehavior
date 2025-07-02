using UnityEngine;
using UnityEngine.AI;

public class WanderingAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destination;

    public float wanderRadius = 10f;
    public float wanderTimer = 10f;

    public float pauseDuration = 2f;

    private bool IsPaused = false;
    private float timer;
    private float pauseTimer;
    private CrowdBehaviorManager crowdManager;
    private FollowLeaderBehavior followBehavior;
    // Vector3[]/ or List<Vector3> points

    private void Start()
    {
        crowdManager = FindObjectOfType<CrowdBehaviorManager>();
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        SetNewRandomDestination();
    }

    private void Update()
    {
        bool isFollowing = followBehavior != null && followBehavior.isFollowingLeader;

        if (!FleeBehavior.isFleeing && !isFollowing)
        {
            if (!IsPaused)
            {
                timer += Time.deltaTime;

                if (timer >= wanderTimer)
                {
                    SetNewRandomDestination();
                }

                if (agent.remainingDistance < 1f)
                {
                    SetNewRandomDestination();
                }
            }
            else
            {
                pauseTimer += Time.deltaTime;

                if (pauseTimer >= pauseDuration)
                {
                    IsPaused = false;
                    SetNewRandomDestination();
                }
            }
        }
        else if (isFollowing && agent.hasPath)
        {
            agent.ResetPath(); // Stop wandering while following
        }
    }
    private void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);

        destination = navHit.position;
        agent.SetDestination(destination);
        timer = 0;

        if (Random.Range(0, 1f) < 0.8f) // 80% chance of stoppping
        {
            IsPaused = true;
            pauseTimer = 0;
        }
    }
    
    public void ForceResumeWandering()
    {
        IsPaused = false;
        pauseTimer = 0f;
        timer = wanderTimer;
        SetNewRandomDestination();
    }

}