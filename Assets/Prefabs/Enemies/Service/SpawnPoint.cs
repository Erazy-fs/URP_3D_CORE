using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SpawnPoint : MonoBehaviour
{
    private Transform targetTransform;

    public void SpawnEnemy(Enemy enemy)
    {
        Debug.Log("SpawnEnemy");
        // Анимация спавна?
        // Спавн врагов в направлении игрока/пенетратора
        // 
        // 
        //float yOffset = -3f;
        //Debug.Log("collider?.bounds.size.y");
        //Debug.Log(collider?.bounds.size.y);
        enemy.isActive = false;
        Enemy enemyEntity = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z), transform.rotation);
        //StartCoroutine(MoveOverTime(enemyEntity));
        Collider collider = enemyEntity.GetComponent<Collider>();
        collider.enabled = false;
        NavMeshAgent agent = enemyEntity.GetComponent<NavMeshAgent>();
        agent.enabled = false;

        enemyEntity.GetComponent<Rigidbody>().AddForce(Vector3.up * 25, ForceMode.VelocityChange);
        EnableColliderAfterDelay(enemyEntity, collider, agent, 500);
    }

    public void SetTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        transform.LookAt(targetTransform);
    }

    public async void EnableColliderAfterDelay(Enemy enemyEntity, Collider collider, NavMeshAgent agent, int delay)
    {
        await Task.Delay(delay);
        collider.enabled = true;
        agent.enabled = true;
        enemyEntity.isActive = true;
    }
}
