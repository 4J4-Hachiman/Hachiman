/*
    Class des quest steps de type location reach
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class QItemLocationReach : MonoBehaviour
{
    [field: SerializeField] private LayerMask detectionLayer;
    [field: SerializeField] private QuestData assignedQuest;
    [field: SerializeField] private int assignedQuestStepIndex;
    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        GetComponent<CapsuleCollider>().includeLayers = detectionLayer;
        GetComponent<CapsuleCollider>().excludeLayers = ~detectionLayer;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (QuestManager.CurrentQuestID == assignedQuest.ID)
        {
            GameEvents.TrigOnLocationReached();
        }
    }
}
