using UnityEngine;
using UnityEngine.AI;

public class GolemAI : MonoBehaviour
{
    public Transform player;
    public float followRadius = 10f;
    public float roamRadius = 5f;
    public float roamInterval = 3f;

    private NavMeshAgent agent;
    private float roamTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        roamTimer = roamInterval;
    }

    void Update()
    {
        roamTimer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If too far, move closer
        if (distanceToPlayer > followRadius)
        {
            agent.SetDestination(player.position);
            return;
        }

        // Roam near the player at intervals
        if (roamTimer >= roamInterval)
        {
            Vector3 roamPos = GetRandomPositionNearPlayer();
            agent.SetDestination(roamPos);
            roamTimer = 0;
        }
    }

    Vector3 GetRandomPositionNearPlayer()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += player.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }
}
