using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //public List<Enemy> enemies;
    public List<Wave> waves;
    public List<Enemy> enemies;
    public float emeniesDelay = 0f;

    void Start()
    {
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        SetEnemiesActivity(false);
        Invoke("ActivateEnemies", emeniesDelay);
    }

    void ActivateEnemies()
    {
        SetEnemiesActivity(true);
    }

    void SetEnemiesActivity(bool active = true)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(active);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
