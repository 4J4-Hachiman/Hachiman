using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class QItemLocationReach : MonoBehaviour
{
    [field: SerializeField] private QuestData assignedQuest;
    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();  
        capsuleCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (QuestManager.GetCurrentQuestID() == assignedQuest.ID)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Quest location reached!!!");
                GameEvents.TrigOnLocationReached();
            }
        }
    }
}
