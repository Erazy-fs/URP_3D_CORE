using System;
using System.Collections;
using UnityEngine;
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
        Enemy enemyEntity = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        //StartCoroutine(MoveOverTime(enemyEntity));
        //Collider collider = enemyEntity.GetComponent<Collider>();
        //collider.enabled = false;
        //enemyEntity.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.VelocityChange);
        //StartCoroutine(EnableColliderAfterDelay(collider, 3f));
    }

    public void SetTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        transform.LookAt(targetTransform);
    }
}
