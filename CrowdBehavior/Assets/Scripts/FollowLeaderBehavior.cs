using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class FollowLeaderBehavior : MonoBehaviour
{
    public Transform leader;
    private NavMeshAgent agent;

    public float followDistance = 3f;

    public static List<FollowLeaderBehavior> currentFollowers = new List<FollowLeaderBehavior>();
    public bool isFollowingLeader { get; private set; } = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (leader == null)
        {
            leader = GameObject.FindWithTag("Leader")?.transform;
        }

        if (currentFollowers.Count < 5)
        {
            currentFollowers.Add(this);
            isFollowingLeader = true;
        }
    }

    void Update()
    {
        if (!FleeBehavior.isFleeing && leader != null && isFollowingLeader)
        {
            float distance = Vector3.Distance(transform.position, leader.position);
            if (distance > followDistance)
            {
                agent.SetDestination(leader.position);
            }
            else
            {
                agent.ResetPath();
            }
        }
    }

    private void OnDestroy()
    {
        if (isFollowingLeader)
        {
            currentFollowers.Remove(this);
        }
    }
}
