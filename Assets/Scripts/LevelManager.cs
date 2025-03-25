using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //public List<Enemy> enemies;
    public List<EnemyWave> waves;
    private int waveIndex = 0;
    //public float emeniesDelay = 0f;

    public void StartNextEnemyWave(List<SpawnPoint> spawnPoints)
    {
        waves[waveIndex++].StartEnemyWave(spawnPoints);
    }

    void Start()
    {
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //SetEnemiesActivity(false);
        //Invoke("ActivateEnemies", emeniesDelay);
    }

    //void ActivateEnemies()
    //{
    //    SetEnemiesActivity(true);
    //}

    //void SetEnemiesActivity(bool active = true)
    //{
    //    foreach (Enemy enemy in enemies)
    //    {
    //        enemy.gameObject.SetActive(active);
    //    }
    //}

    void Update()
    {
        
    }

}
