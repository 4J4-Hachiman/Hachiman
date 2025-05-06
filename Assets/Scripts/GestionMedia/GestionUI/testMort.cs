using System;
using UnityEngine;

public class testMort : MonoBehaviour
{
  // public GameObject canvasMort;

  // void OnTriggerEnter(Collider collider)
  // {
  //     if(collider.gameObject.tag == "Player"){
  //         canvasMort.GetComponent<GestionMort>().ArreterJeu();
  //     }
  // }

  void Update()
  {
    if(Input.GetKeyDown(KeyCode.N)){
        Destroy(gameObject);
    }
  }
}
