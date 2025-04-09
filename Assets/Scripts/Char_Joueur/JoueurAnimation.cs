using System;
using UnityEngine;
using UnityEngine.InputSystem;

/* 
    Class de gestion des animations du joueur grace aux Animator et Input system de Unity;
        - Gestion des Inputs du joueur;
        - Gestion des Animations du joueur;

        - À VENIR ...   

    Par : Sebastien Malo;
    Derniere modification : 02/03/2025;    
*/

public class JoueurAnimation : MonoBehaviour
{
    /* ============================================================== */
    /* ============================================================== */

    /* ------------------ REFERENCES INPUT SYSTEM ------------------ */
    private PlayerControls inputActions;
    private InputAction inputMouvement;
    private InputAction SprintInput;

    /* ------------------- REFERENCES COMPONENTS ------------------- */
    private CharacterController cc;
    private Animator animator;

    /* -------------------- VARIABLES MOUVEMENT -------------------- */
    [Header("Mouvement et saut")]
    [SerializeField] private float vitesseMarche = 2f;
    [SerializeField] private float vitesseSprint = 2f;

    /* Sauts */
    [SerializeField] private float forceSaut = 5f;
    [SerializeField] private float forceGravite = -9.8f;

    // void Awake()
    // {
    //     inputActions = new PlayerControls();
    //     animator = GetComponent<Animator>();
    //     cc = GetComponent<CharacterController>();
    // }
    // private void OnEnable()
    // {
    //     // Activer le ActionMap et se s'abonner aux evenements
    //     inputActions.Enable();
    //     inputMouvement = inputActions.MapNormale.Mouvement;
    //     inputActions.MapNormale.Saut.performed += Saut;
    // }

    // private void OnDisable()
    // {
    //     // Desactiver le ActionMap et se desabonner aux evenements
    //     inputActions.Disable();
    //     inputActions.MapNormale.Saut.performed -= Saut;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     animator.SetFloat("vitesse", cc.velocity.magnitude);
    // }

    // private void Saut(InputAction.CallbackContext ctx)
    // {
    //     Debug.Log(Vector3.up * forceSaut);
    // }
}
