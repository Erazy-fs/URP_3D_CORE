using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public List<Enemy> enemies;

    public void StartEnemyWave(List<SpawnPoint> spawnPoints)
    {
        if (spawnPoints.Count > 0)
        {
            foreach (Enemy enemy in enemies)
            {
                spawnPoints[Random.Range(0, spawnPoints.Count)].SpawnEnemy(enemy);
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
