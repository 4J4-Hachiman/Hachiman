using System;
using UnityEngine;
using UnityEngine.InputSystem;

/* 
    Class de gestion des controles du joueur grace aux Input system de Unity;
        - Gestion des Inputs du joueur;
        - Déplacements basés sur la direction de la caméra;

        - À VENIR ...   

    Par : Yanis Oulmane;
    Derniere modification : 01/03/2025;    
*/


[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(Animator))]
public class JoueursControlSebastien : MonoBehaviour
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
    // -----------------------------------------------------------************************************************************************ Sebastien//
    [SerializeField] private float vitesseMarche = 5f;
    [SerializeField] private float vitesseSprint = 2f;

    /* Sauts */
    [SerializeField] private float forceSaut = 5f;
    [SerializeField] private float forceGravite = -9.8f;

    /* --------------------------- CAMERA --------------------------- */
    [Header("Gestion de la camera")]
    [SerializeField] private Camera cameraJoueur;

    // -----------------------------------------------------------************************************************************************ Sebastien//
    /* -------------------- VARIABLES BOOL -------------------- */
    public bool isArmed;
    public bool canMove;
    public bool isLockedOn;
    /* -------------------- VARIABLES GAMEOBJECT -------------------- */
    public GameObject katana;
    public GameObject katanaInSheath;
    public GameObject cube;

    /* ============================================================== */
    /* ============================================================== */

    // -----------------------------------------------------------************************************************************************ Sebastien//
    public enum HachimanState{
        Idle, 
        Mouving,
        Crouch,
        Guarding,
        Parrying,
        Hurt,
        Dead,
        Attacking
    }

    private HachimanState[] mouvableStates;
    private HachimanState state = HachimanState.Idle;
    // -----------------------------------------------------------************************************************************************ Sebastien_End//

    /* ============================================================== */
    private void Awake()
    {
        // Hachiman mouvableStates initialization
        
        // GameObject initialization
        katana.gameObject.SetActive(false);
        katanaInSheath.gameObject.SetActive(true);
        // Variable initialization
        isLockedOn = false;
        isArmed = false;

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
        // -----------------------------------------------------------************************************************************************ Sebastien//
        inputActions.MapNormale.LightAttack.performed += LightAttack;
        inputActions.MapNormale.HeavyAttack.performed += HeavyAttack;
        inputActions.MapNormale.Unsheath.performed += Unsheath;
        inputActions.MapNormale.LockOn.performed += LockOn;
        inputActions.MapNormale.Guarding.performed += Guarding;
        inputActions.MapNormale.Guarding.canceled += StopGuarding;
    }

    private void OnDisable()
    {
        // Desactiver le ActionMap et se desabonner aux evenements
        inputActions.Disable();
        inputActions.MapNormale.Saut.performed -= Saut;
        // -----------------------------------------------------------************************************************************************ Sebastien//
        inputActions.MapNormale.LightAttack.performed -= LightAttack;
        inputActions.MapNormale.HeavyAttack.performed -= HeavyAttack;
        inputActions.MapNormale.Unsheath.performed -= Unsheath;
        inputActions.MapNormale.LockOn.performed -= LockOn;
        inputActions.MapNormale.Guarding.performed -= Guarding;
        inputActions.MapNormale.Guarding.canceled -= StopGuarding;
    }

    void Update()
    {
        // Modification des parametres de l'animator
        animator.SetFloat("vitesse", cc.velocity.magnitude);
        // -----------------------------------------------------------************************************************************************ Sebastien//
        animator.SetFloat("sideMouvement", inputMouvement.ReadValue<Vector2>().x);
        animator.SetFloat("fowardMouvement", inputMouvement.ReadValue<Vector2>().y);

        CanMove();
        if (isLockedOn == true){
            gameObject.transform.LookAt(cube.transform);
        }
        Debug.Log(canMove);
        // -----------------------------------------------------------************************************************************************ Sebastien//
    }

    void FixedUpdate()
    {
        //Debug.Log(inputMouvement.ReadValue<Vector2>().magnitude);
        //Debug.Log(inputActions.MapNormale.Sprint.ReadValue<float>());
        //Debug.Log(isArmed);
        //Debug.Log(state);
        //Debug.Log(cc.velocity.magnitude);
        Vector3 v = GetDirFromCam();
        if(canMove == true){
            if (v.magnitude > 0)
            {
            transform.forward = Vector3.Slerp(transform.forward, v, Time.fixedDeltaTime * 10);
            }   
            cc.Move(Time.deltaTime * GetDeplcement());
        }
        // -----------------------------------------------------------************************************************************************ Sebastien_End//
        
    }

    /////////////////////////////////////////////////////////////////////
    // FUNCTIONS ////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////
    ///// -----------------------------------------------------------************************************************************************ Sebastien//
    private void ResetState()
    {
        state = HachimanState.Idle;
    }

    private void CanMove()
    {
        switch (state)
        {
            case HachimanState.Idle: 
            canMove = true;
            break;

            case HachimanState.Crouch: 
            canMove = true;
            break;

            case HachimanState.Guarding: 
            canMove = true;
            break;

            default: canMove = false;
            break;
        }
    }

    /////////////////////////////////////////////////////////////////////
    // INPUT EVENTS  SUBSCRIPTION ///////////////////////////////////////
    /////////////////////////////////////////////////////////////////////
    private void Saut(InputAction.CallbackContext ctx)
    {
        Debug.Log(Vector3.up * forceSaut);
    }
    // -----------------------------------------------------------************************************************************************ Sebastien//
    private void LightAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isArmed){
            Debug.Log(ctx);
            state = HachimanState.Attacking;
            animator.SetTrigger("lightAttack");
            
            
        }
    }
    // -----------------------------------------------------------************************************************************************ Sebastien//
    private void HeavyAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isArmed){
            Debug.Log(ctx);
            state = HachimanState.Attacking;
            animator.SetTrigger("heavyAttack");
        }
    }

    // -----------------------------------------------------------************************************************************************ Sebastien//
    private void Unsheath(InputAction.CallbackContext ctx)
    {
        if (ctx.performed){
            Debug.Log(ctx);
            if (isArmed == false){
                Debug.Log("armed");
                isArmed = true;
                animator.SetBool("armed", true);
                katana.gameObject.SetActive(true);
                katanaInSheath.gameObject.SetActive(false);
            } else {
                isArmed = false;
                animator.SetBool("armed", false);
                katana.gameObject.SetActive(false);
                katanaInSheath.gameObject.SetActive(true);
            }
        }
    }

    private void LockOn(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx);
        if (ctx.performed){
            Debug.Log(ctx);
            if (isLockedOn == false){
                Debug.Log("lockedOn");
                isLockedOn = true;
                animator.SetBool("lockedOn", true);
            } else {
                isLockedOn = false;
                animator.SetBool("lockedOn", false);
            }
        }
    }

    private void Guarding(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isArmed){
            Debug.Log(ctx);
            state = HachimanState.Guarding;
            animator.SetBool("guarding", true);
        }
    }
    private void StopGuarding(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled && isArmed){
            Debug.Log(ctx);
            state = HachimanState.Idle;
            animator.SetBool("guarding", false);
        }
    }

    // //////////////////////////////////////////////////////////////////
    //  METHODES CLASS //////////////////////////////////////////////////
    // //////////////////////////////////////////////////////////////////

    /// <summary>
    /// Calcule un vector3 sur les axes du monde relatif à la direction de la caméra et la valeur de l'action 'Mouvement' du joueur.>
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

    /// <summary>
    /// Déplacement réel qui sera appliqué au joueur en prenant en considération toutes les facteur pouvant modifier la vitesse de celui-ci;.
    /// </summary>
    /// <returns>Vector de déplacement dans les axes du monde.</returns>
    private Vector3 GetDeplcement()
    {
        Vector3 deplacement = GetDirFromCam();

        float valMouv = inputMouvement.ReadValue<Vector2>().magnitude;
        float valSprint = inputActions.MapNormale.Sprint.ReadValue<float>();

        deplacement *=  valMouv * vitesseMarche;

        return deplacement;
    }
}