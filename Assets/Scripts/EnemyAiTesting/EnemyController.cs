using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (player != null && agent != null)
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(player.position, path))
            {
                agent.SetDestination(player.position);
            }
            else
            {
                Debug.LogWarning("Цель недостижима!");
            }
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Debug.Log("Враг достиг цели или остановился.");
            }
            Debug.Log($"Remaining Distance: {agent.remainingDistance}");
            Debug.Log($"Path Pending: {agent.pathPending}");
            Debug.Log($"Has Path: {agent.hasPath}");
            Debug.Log($"Is Path Stale: {agent.isPathStale}");
        }

        //if (player != null && agent != null)
        //{
        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(player.position, out hit, 10f, NavMesh.AllAreas))
        //    {
        //        agent.SetDestination(hit.position);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Невозможно найти достижимую точку!");
        //    }
        //}
    }
}