using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public List<Enemy> enemies;

    public void StartEnemyWave(List<SpawnPoint> spawnPoints)
    {
        foreach (Enemy enemy in enemies)
        {
            // Анимация спавна?
            // Спавн врагов в направлении игрока/пенетратора
            // 
            // 
            Instantiate(enemy, spawnPoints[Random.Range(0, spawnPoints.Count)].transform);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
