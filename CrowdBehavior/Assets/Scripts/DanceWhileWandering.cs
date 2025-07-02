using UnityEngine;
using UnityEngine.AI;

public class DanceWhileWandering : MonoBehaviour
{
    private WanderingAgent wander;
    private NavMeshAgent agent;

    private bool isDancing = false;
    private float danceTimer = 0f;
    public float danceDuration = 3f;
    public float chanceToDance = 0.01f; // 10% chance per destination

    private Quaternion originalRotation;

    void Start()
    {
        wander = GetComponent<WanderingAgent>();
        agent = GetComponent<NavMeshAgent>();
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isDancing)
        {
            danceTimer += Time.deltaTime;

            // Spin
            transform.Rotate(Vector3.up * 120f * Time.deltaTime);

            // Tilt side to side
            float tilt = Mathf.Sin(Time.time * 3f) * 10f;
            transform.rotation = originalRotation * Quaternion.Euler(tilt, transform.eulerAngles.y, 0);

            if (danceTimer >= danceDuration)
            {
                isDancing = false;
                transform.rotation = originalRotation;

                if (wander != null)
                {
                    wander.enabled = true;
                    wander.ForceResumeWandering(); 
                }
            }

        }
        else if (!isDancing && !agent.pathPending && agent.remainingDistance <= 0.5f && wander.enabled)
        {
            if (Random.value < chanceToDance)
            {
                wander.enabled = false;
                isDancing = true;
                danceTimer = 0f;
            }
        }
    }
}
