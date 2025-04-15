/*
    Class de gestion globale du jeu
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 12/04/2025;
*/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Custom.CSO;

public class Gamemanager : MonoBehaviour
{       
    public static Gamemanager gamemanagerInstance; 
    private GameObject player;

    [Header("Settings")]
    [field: SerializeField] private Settings manageSettings;

    [Header("Fonctionality Classes")]
    public CombatManager Combat { get; private set; }

    [Header("Enemy Health Bars")]    
    [field: SerializeField] GameObject hpBarParent;
    [field: SerializeField] GameObject hpBarInstance;
    private Pooling hpBarPool;
    private List<GameObject> hpBarActiveList;

    [Header("Enemy Positionning Data")]
    [field: SerializeField] private float surroundDistance;
    [field: SerializeField] private float enemySpaceing;

    [Header("Enemy Pooling")]
    [field: SerializeField] private GameObject enemyInstance;
    [field: SerializeField, Min(5)] private int poolAmount;
    [field: SerializeField] private GameObject enemyPoolParent;
    private Pooling enemyPool;

    private GameObject spawnMain;
    private SpawnGroup[] spawnGroups;

    private List<GameObject> activeEnemies;
    private Queue<GameObject> deadEnemies;

    void Awake()
    {
        if (!gamemanagerInstance)
        {
            gamemanagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 30;


        enemyPool = new Pooling(enemyInstance, poolAmount, enemyPoolParent);
        hpBarPool = new Pooling(hpBarInstance, poolAmount, hpBarParent);

        Combat = new CombatManager(this, surroundDistance, enemySpaceing);

        activeEnemies = new List<GameObject>();
        deadEnemies = new Queue<GameObject>();
        hpBarActiveList = new List<GameObject>();

        InitLevel();
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

            EnemyHpBar newHpBar = hpBarPool.GetFromPool().GetComponent<EnemyHpBar>();
            newHpBar.Init(newEnemy);
            newHpBar.OnTargetDeath += HandleOnTargetdeath;
            newHpBar.gameObject.SetActive(true);
        }
        
        spawnGroup.OnGroupTriggered -= HandleGroupTriggered;
    }

    private void HandleOnTargetdeath(EnemyHpBar instance)
    {
        instance.OnTargetDeath -= HandleOnTargetdeath;
        instance.gameObject.SetActive(false);
        hpBarActiveList.Remove(instance.gameObject);
        hpBarPool.ReturnToPool(instance.gameObject);
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
