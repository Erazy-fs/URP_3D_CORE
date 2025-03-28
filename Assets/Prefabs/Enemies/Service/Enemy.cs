using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Transform zeus;
    public float enemySpeed;
    public float attackDamage;
    public bool isRunner;
    //public float zeusAttackOffset;

    public bool isActive = true;
    private string movingAnimationName;
    private NavMeshAgent agent;
    private Animator animator;
    private Health health;
    private float playerTriggerDistance;

    protected virtual void Start()
    {
        movingAnimationName = isRunner ? "Run" : "Walk";
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.OnHealthChanged += OnHealthChanged;
        playerTriggerDistance = agent.stoppingDistance * 2;

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
        if (zeus == null)
        {
            GameObject zeusObject = GameObject.FindGameObjectWithTag("Zeus");
            if (zeusObject != null)
            {
                zeus = zeusObject.transform;
            }
        }
    }

    void Update()
    {
        agent.speed = (animator?.GetAnimatorTransitionInfo(0).duration ?? 0) == 0 && (animator?.GetCurrentAnimatorStateInfo(0).IsName(movingAnimationName) ?? false) ? enemySpeed : 0;
        Transform target = GetTarget();
        if (isActive && target != null)
        {
            //Debug.Log($"speed: {enemySpeed}");
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(target.position, path))
            {
                agent.SetDestination(target.position);
            }
            else
            {
                //Debug.LogWarning("Цель недостижима!");
            }
            if (animator != null)
            {
                //Debug.Log($"animator.GetAnimatorTransitionInfo(0).duration: {animator.GetAnimatorTransitionInfo(0).duration}");
                bool moving = agent.hasPath && agent.remainingDistance > agent.stoppingDistance + (target == zeus ? agent.stoppingDistance : 0);
                animator.SetBool(movingAnimationName, moving);
                animator.SetBool("Attack", agent.hasPath && agent.remainingDistance <= agent.stoppingDistance + (target == zeus ? agent.stoppingDistance : 0));
                //Debug.Log($"animator.deltaPosition: {animator.deltaPosition}");
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

            //if (target != null && agent != null)
            //{
            //    NavMeshHit hit;
            //    if (NavMesh.SamplePosition(target.position, out hit, 10f, NavMesh.AllAreas))
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

    private Transform GetTarget()
    {
        if (zeus != null && Vector3.Distance(transform.position, zeus.position) < agent.stoppingDistance)
        {
            return zeus;
        }
        if (player != null && (zeus == null || Vector3.Distance(transform.position, player.position) < playerTriggerDistance && GetClosestTarget() == player))
        {
            return player;
        }
        return zeus;
    }

    private void OnHealthChanged(float currentHealth)
    {
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Debug.Log("OnDeath");
        animator?.SetBool("Dead", true);
    }

    // Уничтожить объект красиво
    // Destroy(gameObject); // Удаление объекта при смерти
    public void Goodbye()
    {
        Debug.Log("Goodbye");
        Destroy(gameObject);
    }

    public void Attack()
    {
        Transform target = GetClosestTarget(); // Проверять направление взгляда врага? Или сделать хитбокс перед всеми врагами?
        target.GetComponent<Health>()?.TakeDamage(attackDamage);
    }

    private Transform GetClosestTarget()
    {
        if (zeus == null) return player;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToZeus = Vector3.Distance(transform.position, zeus.position);
        return distanceToPlayer < distanceToZeus ? player : zeus;
    }
}