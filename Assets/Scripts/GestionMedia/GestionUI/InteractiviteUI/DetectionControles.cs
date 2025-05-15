using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectionControles : MonoBehaviour
{
    Vector2 dernierPositionSouris;
    Vector2 positionSouris;
    float changementPositionSouris;
    static bool sourisClavierUtilise;
    public PlayerInput playerInput;

  void Start()
  {
    // sourisClavierUtilise = true;
    // dernierPositionSouris = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
  }
  void Update()
    {
        // positionSouris = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
        
        // changementPositionSouris = Vector2.Distance(dernierPositionSouris, positionSouris);
        // // print(changementPositionSouris);
        // if(changementPositionSouris > 0){
        //     sourisClavierUtilise = true;
        // }
        
        sourisClavierUtilise = playerInput.currentControlScheme == "Keyboard" || playerInput.currentControlScheme == "Mouse";

        print(sourisClavierUtilise + "le currentScheme: " + playerInput.currentControlScheme);
    }
}
