using UnityEngine;

public class CrowdBehaviorManager : MonoBehaviour
{
    public enum BehaviorType { Wander, FollowLeader }
    public BehaviorType currentBehavior = BehaviorType.Wander;

    public float switchInterval = 10f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            SwitchBehavior();
            timer = 0f;
        }
    }

    void SwitchBehavior()
    {
        // Randomly pick a new behavior
        currentBehavior = (Random.value > 0.5f) ? BehaviorType.Wander : BehaviorType.FollowLeader;
        Debug.Log($"Switched behavior to {currentBehavior}");
    }
}
