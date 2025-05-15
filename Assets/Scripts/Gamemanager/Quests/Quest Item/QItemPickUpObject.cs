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
    [field: SerializeField] private RectTransform interactionIcon;
    private PlayerControls playerInputs;
    private Transform cam;

    private void Awake()
    {
        playerInputs = new PlayerControls();
        cam = Camera.main.transform;
        interactionIcon.gameObject.SetActive(false);
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInputs.MapNormale.Interact.performed += Interact;
        if (QuestManager.CurrentQuestID == assignedQuest.ID && QuestManager.CurrentQuestStepIndex == assignedQuestStepIndex)
        {
            interactionIcon.position = gameObject.transform.position + Vector3.up;
            interactionIcon.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        interactionIcon.rotation = Quaternion.LookRotation(cam.transform.forward);
    }

    private void OnTriggerExit(Collider other)
    {
        playerInputs.MapNormale.Interact.performed -= Interact;
        interactionIcon.gameObject.SetActive(false);
    }
}