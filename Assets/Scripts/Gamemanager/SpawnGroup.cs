using System;
using UnityEngine;
/*
    Class de gestion des groupes pour organisation des spawns et
    des patrouilles des ennemis. Plus custom struct pour la structure 
    des patrouilles.
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 28/03/2025;
*/
public class SpawnGroup : MonoBehaviour
{
    [field: SerializeField] private LayerMask mask = 8;
    [field: SerializeField] private bool showGizmos = true;
    [field: SerializeField, Range(0f, 1f)] private float groupTriggerGizmosOpacity = 1;
    [field: SerializeField, Range(0f, 50f)] private float triggerRadius = 10f;
    [field: SerializeField] public PatrolRoute[] patrolRoutes { get; private set; } = new PatrolRoute[0];
    public event Action<SpawnGroup> OnGroupTriggered;

    private void Awake()
    {
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = triggerRadius;
        sphereCollider.includeLayers = mask;
        sphereCollider.excludeLayers = ~mask;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnGroupTriggered?.Invoke(this);
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        Gizmos.color = new Color(1, 0, 0, 1);

        for (int i = 0; i < patrolRoutes.Length; i++)
        {
            if (!patrolRoutes[i].displayRoute) continue;
            Vector3[] pts = patrolRoutes[i].GetPatrolPoints();
            for (int j = 0; j < pts.Length; j++)
            {
                Gizmos.DrawLine(pts[j], pts[j == pts.Length - 1 ? 0 : j + 1]);
                Gizmos.DrawSphere(pts[j], 0.1f);
            }
        }
        Gizmos.color = new Color(0, 1, 0, groupTriggerGizmosOpacity);
        Gizmos.DrawSphere(transform.position, triggerRadius);
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
