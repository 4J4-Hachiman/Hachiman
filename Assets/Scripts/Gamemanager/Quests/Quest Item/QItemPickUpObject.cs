/*
    Logique des Quest steps de type pickup
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider), typeof(PlayerInput))]
public class QItemPickUpObject : MonoBehaviour
{
    [field: SerializeField] private QuestData assignedQuest;
    [field: SerializeField] private int assignedQuestStepIndex;
    PlayerControls playerInputs;

    private void Awake()
    {
        playerInputs = new PlayerControls();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.MapNormale.Interact.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (QuestManager.CurrentQuestID == assignedQuest.ID && QuestManager.CurrentQuestStepIndex == assignedQuestStepIndex)
        {
            playerInputs.Disable();
            playerInputs.MapNormale.Interact.performed -= Interact;
            GameEvents.TrigOnQuestItemPickedUp();
            gameObject.SetActive(false);
            Debug.Log("Item picked up");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInputs.MapNormale.Interact.performed += Interact;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInputs.MapNormale.Interact.performed -= Interact;
    }
}