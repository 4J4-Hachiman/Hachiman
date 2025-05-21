using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectionControles : MonoBehaviour
{
  public static bool sourisClavierUtilise;
  public PlayerInput playerInput;

  void Start()
  {
    sourisClavierUtilise = true;
  }
  void Update()
  {
    sourisClavierUtilise = playerInput.currentControlScheme == "Keyboard" || playerInput.currentControlScheme == "Mouse";
  }
}
