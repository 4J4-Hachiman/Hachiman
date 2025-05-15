/*
    Class de gestion globale du jeu
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025;
*/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Custom.CSO;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour, IDataSaveable
{
    public static Gamemanager gamemanagerInstance;
    private Transform player;
    public Transform cam;

    [field: SerializeField] private SwordData swordData;

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
        Application.targetFrameRate = -1;

        enemyPool = new Pooling(enemyInstance, poolAmount, enemyPoolParent);
        hpBarPool = new Pooling(hpBarInstance, poolAmount, hpBarParent);

        Combat = new CombatManager(this, surroundDistance, enemySpaceing);

        activeEnemies = new List<GameObject>();
        deadEnemies = new Queue<GameObject>();
        hpBarActiveList = new List<GameObject>();

        InitLevel();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void InitLevel()
    {
        Invoke(nameof(LockCursor), 1f);

        player = GameObject.FindGameObjectWithTag("Player").transform;
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

    private void HandleEnemyDeath(EnnemiMain instance)
    {
        instance.OnEnemyDeath -= HandleEnemyDeath;
        activeEnemies.Remove(instance.gameObject);
        deadEnemies.Enqueue(instance.gameObject);
        Combat.EnemyDeath(instance);

        if (activeEnemies.Count == 0)
        {
            GameEvents.TrigAllEnemiesKilled();
            StartCoroutine(UnloadEnemies());
        }
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
        foreach (GameObject enemy in activeEnemies)
        {
            enemy.GetComponent<EnnemiMain>().OnAlertAll -= HandleAlertAll;
            enemy.GetComponent<EnnemiMain>().StartCombat();
        }

        Combat.StartCombat(activeEnemies);
    }

    private IEnumerator UnloadEnemies()
    {
        yield return new WaitForSeconds(5);
        while (deadEnemies.Count > 0)
        {
            enemyPool.ReturnToPool(deadEnemies.Dequeue());
        }
    }

    /* IDataSaveable Interface */
    public void LoadData(GameData data)
    {
        if (data.lScene == SceneManager.GetActiveScene().name)
        {
            player.SetPositionAndRotation(data.playerPosition, data.playerRotation);
        }

        player.GetComponent<JoueursControl1>().numbPotion = data.playerPotionCount;
        
        for (int i = 0; i < swordData.KatanaList.Count; i++)
        {
            if (data.swordState.GetKey(swordData.KatanaList[i].ID, true))
            {
                player.GetComponent<JoueursControl1>().katanaList.Add(swordData.KatanaList[i].gameObject);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = player.position;
        data.playerRotation = player.rotation;
        data.playerPotionCount = player.GetComponent<JoueursControl1>().numbPotion;

        for (int i = 0; i < player.GetComponent<JoueursControl1>().katanaList.Count; i++)
        {
            data.swordState.SetPair(player.GetComponent<JoueursControl1>().katanaList[i].GetComponent<Sword>().ID, true);
        }
    }
}