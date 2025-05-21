/*
    Class de gestion des groupes pour organisation des spawns et
    des patrouilles des ennemis. Plus custom struct pour la structure 
    des patrouilles.
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 18/05/2025;
*/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnGroup : MonoBehaviour, IDataSaveable
{
    [Header("ID")]
    [field: SerializeField] private string groupeID;
    [field: SerializeField] private LayerMask mask = 8;
    [field: SerializeField] private bool showGizmos = true;
    [field: SerializeField, Range(0f, 1f)] private float groupTriggerGizmosOpacity = 1;
    [field: SerializeField] public PatrolRoute[] patrolRoutes { get; private set; } = new PatrolRoute[0];
    public event Action<SpawnGroup> OnGroupTriggered;
    [field: SerializeField] private QuestData assignedQuest;

    private void Awake()
    {
        groupeID = $"SPWNGRP_{SceneManager.GetActiveScene().name}_{gameObject.name.ToLower()}";
        Collider collider = gameObject.GetComponent<Collider>();
        collider.isTrigger = true;
        collider.includeLayers = mask;
        collider.excludeLayers = ~mask;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnGroupTriggered?.Invoke(this);
            GetComponent<Collider>().enabled = false;
        }
    }

    public void AddRoute(Vector3[] newPoints)
    {
        PatrolRoute[] oldRoutes = patrolRoutes;
        patrolRoutes = new PatrolRoute[oldRoutes.Length + 1];
        for (int i = 0; i < oldRoutes.Length; i++)
        {
            patrolRoutes[i] = oldRoutes[i];
        }
        patrolRoutes[^1] = new PatrolRoute(newPoints);
    }

    public void LoadData(GameData data)
    {
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = data.spawnGroupsStates.GetKey(groupeID, collider.enabled);
        if (data.activeQuest == assignedQuest.ID)
        {
            collider.enabled = true;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.spawnGroupsStates.SetPair(groupeID, gameObject.GetComponent<Collider>().enabled);
    }
}

[Serializable]
public struct PatrolRoute
{
    [SerializeField] public bool displayRoute;
    [SerializeField] private bool backAndForth;
    [SerializeField] private Vector3[] patrolPoints;
    public PatrolRoute(Vector3[] patrolPoints, bool backAndForth = false, bool displayRoute = true)
    {
        this.patrolPoints = patrolPoints;
        this.backAndForth = backAndForth;
        this.displayRoute = displayRoute;
    }

    public readonly Vector3[] GetPatrolPoints()
    {
        if (backAndForth)
        {
            Vector3[] newPatrolRoute = new Vector3[patrolPoints.Length * 2 - 1];
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                newPatrolRoute[i] = patrolPoints[i];
                if (i != patrolPoints.Length - 1)
                {
                    newPatrolRoute[newPatrolRoute.Length - 1 - i] = patrolPoints[i];
                }
            }
            return newPatrolRoute;
        }
        return patrolPoints;
    }
}
