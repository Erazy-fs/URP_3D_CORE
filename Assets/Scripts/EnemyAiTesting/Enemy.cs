using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float enemySpeed;
    public string movingAnimationName;

    private NavMeshAgent agent;
    private Animator animator;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

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
            agent.speed = animator.GetAnimatorTransitionInfo(0).duration == 0 ? enemySpeed : 0;
            Debug.Log($"speed: {enemySpeed}");
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(player.position, path))
            {
                agent.SetDestination(player.position);
            }
            else
            {
                //Debug.LogWarning("Цель недостижима!");
            }
            if (animator != null)
            {
                //Debug.Log($"animator.GetAnimatorTransitionInfo(0).duration: {animator.GetAnimatorTransitionInfo(0).duration}");
                bool moving = agent.hasPath && agent.remainingDistance > agent.stoppingDistance;
                animator.SetBool(movingAnimationName, moving);
                animator.SetBool("Attack", agent.hasPath && agent.remainingDistance <= agent.stoppingDistance);
                Debug.Log($"animator.deltaPosition: {animator.deltaPosition}");
                //if (walking)
                //{
                //    agent.speed = (animator.deltaPosition / Time.deltaTime).magnitude + 1;
                //}

            }
            //if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            //{
            //    //Debug.Log("Враг достиг цели или остановился.");
            //}
            //Debug.Log($"Remaining Distance: {agent.remainingDistance}");
            //Debug.Log($"Path Pending: {agent.pathPending}");
            //Debug.Log($"Has Path: {agent.hasPath}");
            //Debug.Log($"Is Path Stale: {agent.isPathStale}");
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