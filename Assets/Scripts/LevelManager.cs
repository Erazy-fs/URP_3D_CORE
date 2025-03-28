using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<EnemyWave> waves;
    private int waveIndex = 0;

#if UNITY_EDITOR
    public GameObject spawnPointsGameObject;
    private List<SpawnPoint> spawnPoints;
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void StartNextEnemyWave(List<SpawnPoint> spawnPoints)
    {
        Debug.Log($"LevelManager.StartNextEnemyWave. spawnPoints.Count: {spawnPoints.Count}");
        if (Instance != null)
        {
            if (Instance.waveIndex < Instance.waves.Count)
            {
                Instance.waves[Instance.waveIndex++].StartEnemyWave(spawnPoints);
            }
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject zeusObject = GameObject.FindGameObjectWithTag("Zeus");
            spawnPoints = spawnPointsGameObject.GetComponentsInChildren<SpawnPoint>().ToList();
            spawnPoints.ForEach(sp => sp.SetTarget(zeusObject.transform));
            LevelManager.StartNextEnemyWave(spawnPoints);
        }
    }
#endif

}
