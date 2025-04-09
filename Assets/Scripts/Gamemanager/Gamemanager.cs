/*
    Class de gestion globale du jeu
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 27/03/2025;
*/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Custom.CSO;

public class Gamemanager : MonoBehaviour
{   
    private GameObject player;
    [field: SerializeField] private Settings manageSettings;

    [Header("Combat Manager")]
    public CombatManager Combat { get; private set; }
    [field: SerializeField] private float surroundDistance;
    [field: SerializeField] private float enemySpaceing;

    [Header("Pooling")]
    [field: SerializeField] private GameObject enemyInstance;
    [field: SerializeField, Min(5)] private int poolAmount;
    [field: SerializeField] private Pooling enemyPool;

    [Header("Spawns")]
    [field: SerializeField] private GameObject spawnMain;
    [field: SerializeField] private SpawnGroup[] spawnGroups;

    [Header("Enemy lists")]
    [field: SerializeField] private List<GameObject> activeEnemies;
    [field: SerializeField] private Queue<GameObject> deadEnemies;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        manageSettings.SetFramerate(80);

        Combat = new CombatManager(this, surroundDistance, enemySpaceing);
        enemyPool = new Pooling(enemyInstance, poolAmount);

        activeEnemies = new List<GameObject>();
        deadEnemies = new Queue<GameObject>();

        InitLevel();
    }
    
    private void InitLevel()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnMain = GameObject.FindWithTag("SpawnData");
        spawnGroups = new SpawnGroup[spawnMain.transform.childCount];
        for (int i = 0; i < spawnGroups.Length; i++)
        {
            spawnGroups[i] = spawnMain.transform.GetChild(i).gameObject.GetComponent<SpawnGroup>();
            spawnGroups[i].OnGroupTriggered += HandleGroupTriggered;
        }
    }


    private void HandleGroupTriggered(SpawnGroup spawnGroup)
    {
        for (int i = 0; i < spawnGroup.patrolRoutes.Length; i++)
        {
            GameObject newEnemy = enemyPool.GetFromPool();
            Vector3[] patrolPoints = spawnGroup.patrolRoutes[i].GetPatrolPoints();

            newEnemy.transform.position = patrolPoints[0];
            newEnemy.SetActive(true);
            newEnemy.GetComponent<EnnemiMain>().Initialize(patrolPoints);
            newEnemy.GetComponent<EnnemiMain>().OnEnemyDeath += HandleEnemyDeath;
            newEnemy.GetComponent<EnnemiMain>().OnAlertAll += HandleAlertAll;
            activeEnemies.Add(newEnemy);
        }

        spawnGroup.OnGroupTriggered -= HandleGroupTriggered;
    }

    private void HandleAlertAll()
    {
        foreach(GameObject enemy in activeEnemies)
        {
            enemy.GetComponent<EnnemiMain>().OnAlertAll -= HandleAlertAll;
            enemy.GetComponent<EnnemiMain>().StartCombat();
        }
    }

    private void HandleEnemyDeath(EnnemiMain instance)
    {
        instance.OnEnemyDeath -= HandleEnemyDeath;
        activeEnemies.Remove(instance.gameObject);
        deadEnemies.Enqueue(instance.gameObject);

        if (activeEnemies.Count == 0)
        {
            Debug.Log("<color=green>All enemies are down</color>");
            StartCoroutine(UnloadEnemies());
        }
    }

    private IEnumerator UnloadEnemies()
    {
        yield return new WaitForSeconds(5);
        while (deadEnemies.Count > 0)
        {
            enemyPool.ReturnToPool(deadEnemies.Dequeue());
        }
    }
}
