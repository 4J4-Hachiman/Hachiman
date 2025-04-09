using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Collections;

/* 
    Class de gestion des controles du joueur grace aux Input system de Unity;
        - Gestion des Inputs du joueur;
        - Déplacements basés sur la direction de la caméra;

        - À VENIR ...   

    Par : Yanis Oulmane;
    Derniere modification : 01/03/2025;    
*/


// [DisallowMultipleComponent]
// [RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(Animator))]
public class JoueursControl : MonoBehaviour
{
    /* ============================================================== */
    /* ============================================================== */

    /* ------------------ REFERENCES INPUT SYSTEM ------------------ */
    private PlayerControls inputActions;
    private InputAction inputMouvement;
    // private InputAction SprintInput;

    /* ------------------- REFERENCES COMPONENTS ------------------- */
    private CharacterController cc;
    private Animator animator;

    /* -------------------- VARIABLES MOUVEMENT -------------------- */
    [Header("Mouvement et saut")]
    [SerializeField] private float vitesseMarche = 2f;
    [SerializeField] private float vitesseSprint = 2f;
    [SerializeField] private float vitesseCrouch = 0.5f;

    /* Sauts */
    [SerializeField] private float forceSaut = 5f;
    [SerializeField] private float forceGravite = -9.8f;
    [SerializeField] private float vitesseTombe = 0.2f; 

    private bool auSol = true;
    private float vyJoueur = 0;
    private Vector3 vJoueur;

    [SerializeField] LayerMask mask;

    /* --------------------------- CAMERA --------------------------- */
    [Header("Gestion de la camera")]
    [SerializeField] private Camera cameraJoueur;

    /* ============================================================== */
    /* ============================================================== */
    private void Awake()
    {
        inputActions = new PlayerControls();

        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        forceGravite = -Math.Abs(forceGravite);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputMouvement = inputActions.MapNormale.Mouvement;
        inputActions.MapNormale.Saut.performed += Saut;
    }

    private void OnDisable()
    {
        // Desactiver le ActionMap et se desabonner aux evenements
        inputActions.Disable();
        inputActions.MapNormale.Saut.performed -= Saut;
    }


    /* ================================ METHODES UPDATE ================================ */

    void Update()
    {
        // Modification des parametres de l'animator
        animator.SetFloat("vitesse", cc.velocity.magnitude);
    
        auSol = GetAuSol();

        // Maj de la velocite du joueur
        vJoueur = GetDeplacement();
        vJoueur.y = vyJoueur;
    }

    void FixedUpdate()
    {
        if (vJoueur.magnitude > 0)
        {
            transform.forward = Vector3.Slerp(transform.forward, GetDirFromCam(), Time.fixedDeltaTime * 10);
        }

        vyJoueur += forceGravite * Time.fixedDeltaTime;
        cc.Move(Time.deltaTime * vJoueur);
    }



    /* ================================ INPUTS CALLBACK ================================ */

    private void Saut(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jumped");

        vyJoueur += forceSaut + Mathf.Abs(forceGravite);
    }


    /* ===================== METHODES POUR LE DÉPLACEMENT DU JOUEUR ===================== */

    /// <summary>
    /// Calcule un vector3 sur les axes du monde relatif à la direction de 
    /// la caméra et la valeur de l'action 'Mouvement' du joueur.
    /// </summary>
    /// <returns>Direction vers lequel le joueur se déplacera.</returns>
    private Vector3 GetDirFromCam()
    {
        Vector2 inputMouv = inputMouvement.ReadValue<Vector2>();

        Vector3 v = Vector3.zero;

        Vector3 vFoward = cameraJoueur.transform.forward;
        vFoward.y = 0;
        vFoward.Normalize();

        Vector3 vRight = cameraJoueur.transform.right;
        vRight.y = 0;
        vRight.Normalize();

        v = (vFoward * inputMouv.y) + (vRight * inputMouv.x);

        return v;
    }

    /// summary :
    /// Déplacement réel qui sera appliqué au joueur en prenant en 
    /// considération toutes les facteur pouvant modifier la vitesse de celui-ci;.
    /// </summary>
    /// <returns>Vector de déplacement dans les axes du monde.</returns>
    /// 
    private Vector3 GetDeplacement()
    {
        Vector3 deplacement = GetDirFromCam();

        float vFact = 1;

        float valMouv = inputMouvement.ReadValue<Vector2>().magnitude;
        float valSprint = inputActions.MapNormale.Sprint.ReadValue<float>();
        // float valCrouch = inputActions.MapNormale.Crouch.ReadValue<float>();


        vFact *= valMouv * vitesseMarche;

        vFact = valSprint > 0 ? valMouv * vitesseSprint : vFact;

        // vFact = valCrouch > 0 ? valMouv * vitesseCrouch : vFact;

        // Solution one line complique
        // deplacement *= Mathf.Pow(Mathf.Pow(valMouv * vitesseMarche, 1 - valSprint) * Mathf.Pow(vitesseSprint, valSprint), 1 - valCrouch) * Mathf.Pow(vitesseCrouch, Mathf.Abs(0 - valCrouch));

        return deplacement * vFact;
    }

    private bool GetAuSol()
    {
        // string coloredText = "<color=green> Player is grounded </color> and touched";
        if (Physics.SphereCast(transform.position + Vector3.up * cc.radius, cc.radius, Vector3.down, out RaycastHit hitInfo, cc.radius))
        {
            if (vyJoueur < forceGravite)
            {
                vyJoueur = forceGravite;
            }
            // vyJoueur = forceGravite;
            // Debug.Log($"<color=green>Grounded</color> and touching <color=blue>{hitInfo.transform.name}</color>");
            return true;
        }
        else
        {
            // Debug.Log($"<color=orange>Not touching anything</color>");
            if (auSol)
            {
                return false;
            }
            StartCoroutine(GestionGravite());
            return false;
        }
    }

    IEnumerator GestionGravite()
    {
        // v = -9.8m/s^2
        
        while (!auSol)
        {
            vyJoueur += forceGravite * Time.deltaTime * vitesseTombe;

            yield return new WaitForSeconds(1/60);
        }

        vyJoueur = forceGravite;

        yield break;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.27f, 0.27f);
    }

    // public static string ColoredText<T>(T data, string color)
    // {
    //     return $"<color={color}>{data}</color>";
    // }
}