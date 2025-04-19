using UnityEngine;
using UnityEngine.InputSystem;

public class QItemPickUpObject : MonoBehaviour
{
    [field: SerializeField] private QuestData assignedQuest;
    [field: SerializeField] private int assignedQuestStepIndex;
    PlayerControls playerInputs;

    private void Awake()
    {
        playerInputs = new PlayerControls();
        Debug.Log("Quest Item pickup ready");
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
        if (QuestManager.CurrentQuestID != assignedQuest.ID)
        {
            Debug.Log("Wrong quest");
            return;
        }

        playerInputs.Disable();
        playerInputs.MapNormale.Interact.performed -= Interact;
        GameEvents.TrigOnQuestItemPickedUp();
        Destroy(gameObject.GetComponent<PlayerInput>());
        Debug.Log("Item picked up");
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