/*
    Logique des Quest steps de type pickup
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider), typeof(PlayerInput))]
public class QItemPickUpObject : MonoBehaviour
{
    [field: SerializeField] private List<QuestData> assignedQuests;
    private List<string> assignedQuestID;
    public RectTransform interactionIcon;
    [field: SerializeField] private float heightDiff;

    private PlayerControls playerInputs;
    private Transform cam;

    private void Awake()
    {
        playerInputs = new PlayerControls();
        cam = Camera.main.transform;
        assignedQuestID = new();

        for (int i = 0; i < assignedQuests.Count; i++)
        {
            assignedQuestID.Add(assignedQuests[i].ID);
        }

        enabled = true;
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        interactionIcon = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gamemanager>().InteractionIcon;
        interactionIcon.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.MapNormale.Interact.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        playerInputs.Disable();
        playerInputs.MapNormale.Interact.performed -= Interact;
        GameEvents.TrigOnQuestItemPickedUp();
        interactionIcon.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (assignedQuestID.Contains(QuestManager.CurrentQuestID))
        {
            interactionIcon.gameObject.SetActive(true);
            interactionIcon.position = transform.position + (Vector3.up * heightDiff);
            playerInputs.MapNormale.Interact.performed += Interact;
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