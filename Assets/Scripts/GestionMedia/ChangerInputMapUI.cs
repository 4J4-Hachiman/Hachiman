using UnityEngine;
using UnityEngine.InputSystem;

public class ChangerInputMapUI : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputActionAsset inputActionAsset;
    public bool changerInputUI;
    public bool switchFait;

    private void Start()
    {
        changerInputUI = false;
        switchFait = false;
        // Make sure the PlayerInput component is attached to the same object
        // if (playerInput == null)
        // {
        //     playerInput = GetComponent<PlayerInput>();
        // }

        // if (changerInputUI && !switchFait)
        // {
        //     ChangerInputUI();
        // }
        // else if (!changerInputUI && !switchFait)
        // {
        //     ChangerInputMouvement();
        // }
    }

    public void ChangerInputMouvement()
    {
        if (playerInput != null)
        {
            changerInputUI = false;
            playerInput.SwitchCurrentActionMap("MapNormale");
        }
    }

    public void ChangerInputUI()
    {
        if (playerInput != null)
        {
            changerInputUI = true;
            playerInput.SwitchCurrentActionMap("UImap");
        }
    }
}
