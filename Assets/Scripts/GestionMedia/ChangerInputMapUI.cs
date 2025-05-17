   using UnityEngine;
   using UnityEngine.InputSystem;

   public class ChangerInputMapUI : MonoBehaviour
   {
       public PlayerInput playerInput;
       public InputActionAsset inputActionAsset;

       private void Start()
       {
           // Make sure the PlayerInput component is attached to the same object
           if (playerInput == null)
           {
               playerInput = GetComponent<PlayerInput>();
           }
       }

       public void ChangerInputMouvement()
       {
           if (playerInput != null)
           {
               // Use the name of your Action Map
               playerInput.SwitchCurrentActionMap("MapNormale");
           }
       }

       public void ChangerInputUI()
       {
           if (playerInput != null)
           {
               playerInput.SwitchCurrentActionMap("UImap");
           }
       }
   }
