using UnityEngine;
using UnityEngine.InputSystem;


public class Portal : MonoBehaviour
{
    public SceneActiveManager sceneActiveManager;
    public JoueursControl1 joueursControl1;
    private bool inFrontOfPortal;

    /* ------------------ REFERENCES INPUT SYSTEM ------------------ */
    private PlayerControls inputActions;
    private InputAction inputMouvement;
    private InputAction SprintInput;

    void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Enable();
        inputActions.MapNormale.Interact.performed += Interact;
    }

    void Start()
    {
        inFrontOfPortal = false;
    }

    void Update()
    {
        
    }
    
    void OnTriggerStay()
    {
        inFrontOfPortal = true;
    }

    void OnTriggerExit()
    {
        inFrontOfPortal = false;
    }

    void Teleport()
    {
        sceneActiveManager.ChargerScene("Scene_Boss");
    }
    private void Interact(InputAction.CallbackContext context)
    {
        if (inFrontOfPortal)
        {
            Teleport();
        }
    }
}
