using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //public List<EnemyController> enemies; 
    private GameObject[] enemies;
    public float emeniesDelay = 0f;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        SetEnemiesActivity(false);
        Invoke("ActivateEnemies", emeniesDelay);
    }

    void ActivateEnemies()
    {
        SetEnemiesActivity(true);
    }

    void SetEnemiesActivity(bool active = true)
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(active);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
