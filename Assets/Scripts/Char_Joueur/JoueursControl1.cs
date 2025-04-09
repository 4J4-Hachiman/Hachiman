using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Collections;
using Unity.Cinemachine;

/* 
    Class de gestion des controles du joueur grace aux Input system de Unity;
        - Gestion des Inputs du joueur;
        - Déplacements basés sur la direction de la caméra;

        - À VENIR ...   

    Par : Yanis Oulmane et Sebastien Malo;
    Derniere modification : 07/03/2025;    
*/


[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(Animator))]
public class JoueursControl1 : MonoBehaviour
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
    public CinemachineVirtualCamera virtualCamera;
    private CameraTarget cameraTarget;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private LayerMask enemyLayer;
    public BanqueAudio banqueAudio;
    public GestionQuete quete;

    /* -------------------- VARIABLES MOUVEMENT -------------------- */
    [Header("Mouvement et saut")]
    [SerializeField] private float currentSpeed = 0f; 
    [SerializeField] private float currentSideMovement;
    [SerializeField] private float currentFowardMovement;
    [SerializeField] private float smoothTime = 5f;
    [SerializeField] private float transitionSpeed = 5f;
    [SerializeField] private float vitesseMarche = 2f;
    [SerializeField] private float vitesseSprint = 2f;
    [SerializeField] private float vitesseCrouch = 0.5f;

    /* Sauts */
    [SerializeField] private float forceSaut = 5f;
    [SerializeField] private float forceGravite = -9.8f;
    [SerializeField] private float vitesseTombe = 0.2f; 
    [SerializeField] private float rotationSpeed = 10f; 

    public float jumpCheckOffset = 0.05f;
    public float gravityCheckOffset = 0.2f;
    public float checkRadius = 0.2f;

    private bool auSol = true;
    private float checkDistance = 0.6f;
    private float vyJoueur = 0;
    private Vector3 vJoueur;
    [SerializeField] private float smallDistance = 10f;

    /* --------------------------- CAMERA --------------------------- */
    [Header("Gestion de la camera")]
    [SerializeField] private Camera cameraJoueur;

    // -----------------------------------------------------------************************************************************************ Sebastien//
    /* -------------------- VARIABLES BOOL -------------------- */
    private bool isArmed;
    private bool isJumping;
    private bool canJump;
    private bool isLockedOn;
    private bool isCrouched;
    private bool isRolling;
    private bool hasJumped;
    private bool hasLanded;
    private bool canMove;
    private bool isDead;
    private bool isGrounded;

    // AttackCombos
    private int comboStep = 0;
    private int combo2Step = 0;
    private int maxCombos = 3;
    private int maxCombos2 = 2;
    private bool isCombo = false;
    private float attackDelay = 1f;
    private string lightAttackCombo;
    private string heavyAttackCombo;
    private List<string> attackCombosList = new List<string>();

    static public int health = 100;

    /* -------------------- VARIABLES GAMEOBJECT -------------------- */
    public GameObject activeKatana;
    public GameObject katana;
    public GameObject katanaInSheath;
    public GameObject cube;
    public GameObject camera;
    public Transform head;

    /* -------------------- VARIABLES ARRAY -------------------- */
    public GameObject[] lockOnOptions; 

    /* ------------------------ AUDIO SOURCE ------------------------- */ 
    public AudioSource[] audioSources;

    /* ============================================================== */
    /* ============================================================== */

    // -----------------------------------------------------------************************************************************************ Sebastien//
    public enum HachimanState{
        Idle, 
        Equiping,
        Healing,
        Jumping,
        Mouving,
        Crouching,
        Rolling,
        Guarding,
        Parrying,
        Hurt,
        Dead,
        Attacking
    }

    private HachimanState[] mouvableStates;
    public HachimanState state = HachimanState.Idle;
    // -----------------------------------------------------------************************************************************************ Sebastien_End//


    private void Awake()
    {
        // AudioSources initialization
        audioSources = GetComponents<AudioSource>();
        // Hachiman mouvableStates initialization
        
        // GameObject initialization
        activeKatana = katana;
        katana.gameObject.SetActive(false);
        katanaInSheath.gameObject.SetActive(true);
        // Variable initialization
        isLockedOn = false;
        isArmed = false;
        isJumping = false;
        isCrouched = false;
        isRolling = false;
        isDead = false;
        canJump = true;

        comboStep = Mathf.Clamp(comboStep, 0, 3);
        lightAttackCombo = "lightAttack";
        heavyAttackCombo = "heavyAttack";
        
        inputActions = new PlayerControls();
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        forceGravite = -Math.Abs(forceGravite);

        cameraTarget.LookAtTarget = cube.transform;  
        cameraTarget.CustomLookAtTarget = true;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputMouvement = inputActions.MapNormale.Mouvement;
        inputActions.MapNormale.Saut.performed += Saut;
        inputActions.MapNormale.Crouch.performed += Crouch;
        inputActions.MapNormale.Roll.performed += Roll;
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
        inputActions.MapNormale.Crouch.performed -= Crouch;
        inputActions.MapNormale.Roll.performed -= Roll;
        inputActions.MapNormale.LightAttack.performed -= LightAttack;
        inputActions.MapNormale.HeavyAttack.performed -= HeavyAttack;
        inputActions.MapNormale.Unsheath.performed -= Unsheath;
        inputActions.MapNormale.LockOn.performed -= LockOn;
        inputActions.MapNormale.Guarding.performed -= Guarding;
        inputActions.MapNormale.Guarding.canceled -= StopGuarding;
    }


    /* ================================ METHODES UPDATE ================================ */

    void Update()
    {
        if (health <= 0)
        {
            Invoke("DiedUI", 2f);
        }
        //Debug.DrawRay(head.position, Vector3.up * checkDistance, Color.red);
        //Debug.Log("attackCombosList Count: " + attackCombosList.Count);
        //Debug.Log("comboStep: " + comboStep);
        //Debug.Log("isCombo: " + isCombo);

        // Modification des parametres de l'animator

        animator.SetFloat("vitesse", cc.velocity.magnitude);
        if (state == HachimanState.Guarding){
            animator.SetFloat("sideMouvement", Mathf.Clamp(inputMouvement.ReadValue<Vector2>().x, -1f, 1f));
            animator.SetFloat("fowardMouvement", Mathf.Clamp(inputMouvement.ReadValue<Vector2>().y, -1f, 1f));
        }
        else 
        {
            // float targetValueXsideMovement = inputMouvement.ReadValue<Vector2>().x;
            // currentSideMovement = Mathf.MoveTowards(currentSideMovement, targetValueXsideMovement, Time.deltaTime);
            // animator.SetFloat("sideMouvement", currentSideMovement);

            // float targetValueYfowardMovement = inputMouvement.ReadValue<Vector2>().y;
            // currentFowardMovement = Mathf.MoveTowards(currentFowardMovement, targetValueYfowardMovement, Time.deltaTime);
            // animator.SetFloat("fowardMouvement", currentFowardMovement);

            animator.SetFloat("sideMouvement", inputMouvement.ReadValue<Vector2>().x * cc.velocity.magnitude);
            animator.SetFloat("fowardMouvement", inputMouvement.ReadValue<Vector2>().y * cc.velocity.magnitude);
        }
    
        CanMove();

        CapsuleCastFromCamera();


        if (isLockedOn == true)
        {
            // Look at object
            Transform target = null;
            Collider[] hits = CapsuleCastFromCamera();

            if (hits.Length > 0 && hits[0].transform != null)
            {
                target = hits[0].transform;

                Vector3 direction = target.position - transform.position;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);
            }

            if (target == null)
            {
                inputActions.MapNormale.Saut.performed += Saut;
                isLockedOn = false;
                animator.SetBool("lockedOn", false);
            }
        }

        Vector3 bottomCenter = transform.position + cc.center - new Vector3(0, cc.height / 2f, 0);
        Vector3 jumpCheckPosition = bottomCenter + Vector3.up * jumpCheckOffset;
        Vector3 gravityCheckPosition = bottomCenter - Vector3.up * gravityCheckOffset;

        canJump = Physics.CheckSphere(jumpCheckPosition, checkRadius, Ground);
        isGrounded = Physics.CheckSphere(gravityCheckPosition, checkRadius, Ground);

        if (!isGrounded)
        {
            StartCoroutine(GestionGravite());
        }
        else if (vyJoueur < forceGravite) 
        {
            vyJoueur = forceGravite;
        }

        if (!isGrounded){
            isJumping = true;
        } 
        if (auSol)
        {
            isJumping = false;
        }

        if (isGrounded && !canJump){
            Landed();
        } 
        if (!isGrounded && canJump){
            Jump();
        } 

        // Maj de la velocite du joueur
        vJoueur = GetDeplacement();
        vJoueur.y = vyJoueur;

        // camera logic
        // virtualCamera.Follow = cameraTarget.TrackingTarget;
        
        // if (cameraTarget.CustomLookAtTarget)
        // {
        //     virtualCamera.LookAt = cameraTarget.LookAtTarget; // Look at the enemy
        // }
        // else
        // {
        //     virtualCamera.LookAt = cameraTarget.TrackingTarget; // Look at the player if no enemy
        // }
    }

    void FixedUpdate()
    {
        if(canMove == true){
            if (vJoueur.magnitude > 0)
            {
                transform.forward = Vector3.Slerp(transform.forward, GetDirFromCam(), Time.fixedDeltaTime * 10);
            }
            cc.Move(Time.deltaTime * vJoueur);
        }
        vyJoueur += forceGravite * Time.fixedDeltaTime;
    }

    /////////////////////////////////////////////////////////////////////
    // FUNCTIONS ////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////

    Collider[] CapsuleCastFromCamera()
    {
        Camera cam = Camera.main;

        // Parameters
        float maxDistance = 20f;
        float coneAngle = 45f; // Half angle (so cone is 90° total)
        float radius = maxDistance; // Detection radius at the end of the cone

        Vector3 origin = cam.transform.position;
        Vector3 direction = cam.transform.forward;

        // OverlapSphere to get all potential targets
        Collider[] hits = Physics.OverlapSphere(origin, radius, enemyLayer);
        foreach(Collider col in hits){
            Debug.Log(col.gameObject);
        }

        foreach (Collider col in hits)
        {
            Vector3 toTarget = col.transform.position - origin;
            float distanceToTarget = toTarget.magnitude;

            // Check if within range
            if (distanceToTarget > maxDistance) continue;

            // Normalize vector to target
            Vector3 toTargetDir = toTarget.normalized;

            // Check angle using dot product
            float angleToTarget = Vector3.Angle(direction, toTargetDir);
            if (angleToTarget <= coneAngle)
            {
                Debug.Log("Cone hit: " + col.name);
                Debug.DrawLine(origin, col.transform.position, Color.green);
            }
        }

        // Optional debug: draw the cone bounds
        Debug.DrawRay(origin, Quaternion.Euler(0, coneAngle, 0) * direction * maxDistance, Color.yellow);
        Debug.DrawRay(origin, Quaternion.Euler(0, -coneAngle, 0) * direction * maxDistance, Color.yellow);

        return hits;
    }

    public void ManageSwordCollider(int state)
    {
        activeKatana.GetComponent<CapsuleCollider>().enabled = state == 1;
    }
    void Death()
    {
        isDead = true;
        animator.SetTrigger("Death");
        inputActions.Disable();
        DoNotListenToInputs();
    }
    
    void NotHit()
    {
        animator.SetBool("Hit", false);
        ListenToInputs();
    }
    
    void DisableRootMotion()
    {
        animator.applyRootMotion = false;
    }

    void EnableRootMotion()
    {
        animator.applyRootMotion = true;
    }

    bool IsObstructedAbove()
    {
        return Physics.Raycast(head.position, Vector3.up, checkDistance);
    }
    private void ResetState()
    {
        state = HachimanState.Idle;
    }

    private void ResetCombo1()
    {
        comboStep = 0;
        attackCombosList.Clear();
        animator.SetInteger("ComboStep", 0);
        isCombo = false;
        ResetState();
        ListenToInputs();
    }

    private void ResetCombo2()
    {
        combo2Step = 0;
        attackCombosList.Clear();
        animator.SetInteger("Combo2Step", 0);
        isCombo = false;
        ResetState();
        ListenToInputs();
    }

    private void ResetRoll()
    {
        state = HachimanState.Idle;
        isRolling = false;
        ListenToInputs();
    }

    private void Jump()
    {
        hasJumped = true;
        hasLanded = false;
        DoNotListenToInputs();
        animator.SetBool("jumping", true);
        state = HachimanState.Jumping;
    }
    private void Landed()
    {
        hasJumped = false;
        hasLanded = true;
        ListenToInputs();
        state = HachimanState.Idle;
        animator.SetBool("jumping", false);
    }

    private void CanMove()
    {
        switch (state)
        {
            case HachimanState.Idle: 
            canMove = true;
            break;

            case HachimanState.Crouching: 
            canMove = true;
            break;

            case HachimanState.Guarding: 
            canMove = true;
            break;

            case HachimanState.Jumping: 
            canMove = true;
            break;

            default: canMove = false;
            break;
        }
    }
    private void DoNotListenToInputs()
    {
        inputActions.MapNormale.Saut.performed -= Saut;
        inputActions.MapNormale.Crouch.performed -= Crouch;
        inputActions.MapNormale.Roll.performed -= Roll;
        inputActions.MapNormale.LightAttack.performed -= LightAttack;
        inputActions.MapNormale.HeavyAttack.performed -= HeavyAttack;
        inputActions.MapNormale.Unsheath.performed -= Unsheath;
        inputActions.MapNormale.LockOn.performed -= LockOn;
        inputActions.MapNormale.Guarding.performed -= Guarding;
        inputActions.MapNormale.Guarding.canceled -= StopGuarding;
    }
    private void ListenToInputs()
    {
        inputActions.MapNormale.Saut.performed += Saut;
        inputActions.MapNormale.Crouch.performed += Crouch;
        inputActions.MapNormale.Roll.performed += Roll;
        inputActions.MapNormale.LightAttack.performed += LightAttack;
        inputActions.MapNormale.HeavyAttack.performed += HeavyAttack;
        inputActions.MapNormale.Unsheath.performed += Unsheath;
        inputActions.MapNormale.LockOn.performed += LockOn;
        inputActions.MapNormale.Guarding.performed += Guarding;
        inputActions.MapNormale.Guarding.canceled += StopGuarding;
    }

    /* ================================ COUROUTINES ================================ */
    IEnumerator ApplyVelocityOnHeavyAttack()
    {
        Vector3 vHeavyAttack = Vector3.zero;

        Vector3 vFoward = cameraJoueur.transform.forward;
        vFoward.y = 0;
        vFoward.Normalize();

        vHeavyAttack = (vFoward * 5f);

        cc.Move(Time.deltaTime * vHeavyAttack);
        
        yield return new WaitForSeconds(0.20f);

        yield return null;
    }
    IEnumerator RollCoroutine(float distance, float duration)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + transform.forward * distance;
        float elapsedTime = 0f;

        // Optional: Raycast to check if there's an obstacle ahead
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance))
        {
            Debug.Log("Obstacle detected: " + hit.collider.name);
            targetPos = hit.point; // Stop at the obstacle
        }

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        transform.position = targetPos;  // Ensure the final position is accurate
        ResetRoll();
    }

    IEnumerator MoveToPosition(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // Ensure the final position is accurate
    }

    /* ================================ INPUTS CALLBACK ================================ */

    // private void Saut(InputAction.CallbackContext ctx)
    // {
    //     if (ctx.performed && auSol && !isJumping)
    //     {
    //         isJumping = true;
    //         vyJoueur += forceSaut + Mathf.Abs(forceGravite);
    //     }
    //     //Debug.Log("Jumped");
    // }

    private void Saut(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canJump && isGrounded)
        {
            canJump = false;
            isGrounded = false;
            
            vyJoueur += forceSaut + Mathf.Abs(forceGravite);
        }
        //Debug.Log("Jumped");
    }
    private void Crouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed){
            if (!IsObstructedAbove())
            {
                state = HachimanState.Crouching;
                //Debug.Log(ctx);
                if (isCrouched == false){
                    Debug.Log("crouch");
                    isCrouched = true;
                    animator.SetBool("crouch", true);
                    cc.center = new Vector3(0, 0.603f, 0);
                    cc.height = 1.206f;
                    DoNotListenToInputs();
                    inputActions.MapNormale.Crouch.performed += Crouch;
                    // remove lock on
                    isLockedOn = false;
                    animator.SetBool("lockedOn", false);
                } else {
                    ListenToInputs();
                    isCrouched = false;
                    animator.SetBool("crouch", false);
                    cc.center = new Vector3(0, 0.905f, 0);
                    cc.height = 1.81f;
                }
            }
        }
    }

    private void Roll(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isRolling)
        {
            isRolling = true;
            animator.SetTrigger("roll");
            state = HachimanState.Rolling;
            DoNotListenToInputs();
            //StartCoroutine(RollCoroutine(2f, 0.54f));
        }
    }

    // -----------------------------------------------------------************************************************************************ Sebastien//
    private void LightAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isArmed){
            // ---------- State ---------
            state = HachimanState.Attacking;
            animator.SetTrigger("lightAttack");
            //Time.timeScale = 0.05f;
            DoNotListenToInputs();
            inputActions.MapNormale.LightAttack.performed += LightAttack;
            inputActions.MapNormale.HeavyAttack.performed += HeavyAttack;
            if (attackCombosList.Count < maxCombos)
            {
                attackCombosList.Add("lightAttack");
            }
            
            if (isCombo == false)
            {
                isCombo = true;
                comboStep += 1;
                StartCoroutine(AttackCombo1());
            }
        }
    }

    // -----------------------------------------------------------************************************************************************ Sebastien//
    private void HeavyAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isArmed){
            //Debug.Log(ctx);
            state = HachimanState.Attacking;
            animator.SetTrigger("heavyAttack");
            
            DoNotListenToInputs();
            inputActions.MapNormale.LightAttack.performed += LightAttack;
            inputActions.MapNormale.HeavyAttack.performed += HeavyAttack;
            if (attackCombosList.Count < maxCombos2)
            {
                attackCombosList.Add("heavyAttack");
            }

            if (isCombo == false)
            {
                isCombo = true;
                combo2Step += 1;
                //StartCoroutine(ApplyVelocityOnHeavyAttack());
                StartCoroutine(AttackCombo2());
            }
        }
    }

    // -----------------------------------------------------------************************************************************************ Sebastien//
    private void Unsheath(InputAction.CallbackContext ctx)
    {
        if (ctx.performed){
            state = HachimanState.Equiping;
            //Debug.Log(ctx);
            if (isArmed == false){
                Debug.Log("armed");
                isArmed = true;
                animator.SetBool("armed", true);
                activeKatana.gameObject.SetActive(true);
                katanaInSheath.gameObject.SetActive(false);
            } else {
                isArmed = false;
                animator.SetBool("armed", false);
                activeKatana.gameObject.SetActive(false);
                katanaInSheath.gameObject.SetActive(true);
            }
        }
    }

    private void LockOn(InputAction.CallbackContext ctx)
    {
        //Debug.Log(ctx);
        if (ctx.performed){
            Debug.Log(ctx);
            if (isLockedOn == false){
                inputActions.MapNormale.Saut.performed -= Saut;
                Debug.Log("lockedOn");
                isLockedOn = true;
                animator.SetBool("lockedOn", true);
            } else {
                inputActions.MapNormale.Saut.performed += Saut;
                isLockedOn = false;
                animator.SetBool("lockedOn", false);
            }
        }
    }

    private void Guarding(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isArmed){
            //Debug.Log(ctx);
            state = HachimanState.Guarding;
            animator.SetBool("guarding", true);
            inputActions.MapNormale.Saut.performed -= Saut;
            inputActions.MapNormale.Crouch.performed -= Crouch;
            inputActions.MapNormale.LightAttack.performed -= LightAttack;
            inputActions.MapNormale.HeavyAttack.performed -= HeavyAttack;
            inputActions.MapNormale.Unsheath.performed -= Unsheath;
            inputActions.MapNormale.LockOn.performed -= LockOn;
        }
    }
    private void StopGuarding(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled && isArmed){
            //Debug.Log(ctx);
            state = HachimanState.Idle;
            animator.SetBool("guarding", false);
            animator.SetBool("Hit", false);
            ListenToInputs();
        }
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

        float valMouv = inputMouvement.ReadValue<Vector2>().magnitude;
        float valSprint = inputActions.MapNormale.Sprint.ReadValue<float>();
        float valCrouch = inputActions.MapNormale.Crouch.ReadValue<float>();

        float targetSpeed = valMouv * vitesseMarche;

        if (!isCrouched && state != HachimanState.Guarding)
        {
            targetSpeed = valSprint > 0 ? valMouv * vitesseSprint : targetSpeed;
        }

        if (isCrouched)
        {
            targetSpeed = valCrouch > 0 ? valMouv * vitesseCrouch : targetSpeed;
        }

        // Transition en douceur entre l'ancienne vitesse et la nouvelle
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * transitionSpeed);

        return deplacement * currentSpeed;
    }
    // private bool GetAuSol() 
    // {
    //     // Vérifie si le joueur est au sol en effectuant un SphereCast sous lui.
    //     if (Physics.SphereCast(transform.position + Vector3.up * cc.radius, cc.radius, Vector3.down, out RaycastHit hitInfo, cc.radius)) 
    //     {
    //         // Si le joueur touche une surface sous lui
    //         if (vyJoueur < forceGravite) 
    //         {
    //             vyJoueur = forceGravite;
    //         }
    //         return true; // Le joueur est au sol.
    //     } 
    //     else 
    //     {
    //         // Si aucun contact avec le sol
    //         if (auSol) 
    //         {
    //             // Si l'état précédent était "au sol", on retourne false.
    //             return false;
    //         }
    //         // Si le joueur est en l'air, on lance une coroutine pour gérer la gravité.
    //         StartCoroutine(GestionGravite());
    //         return false; // Le joueur n'est pas au sol.
    //     }
    // }

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

    IEnumerator AttackCombo2()
    {
        animator.SetInteger("Combo2Step", combo2Step);

        yield return new WaitForSeconds(attackDelay);

        if (attackCombosList.Count > 1)
        {
            if (attackCombosList[1] == "heavyAttack") // If clicked within time window, continue combo
            {
                combo2Step += 1;
                animator.SetTrigger("heavyAttack");
                animator.SetInteger("Combo2Step", combo2Step);
            }
        }
        else
        {
            ResetCombo2();
            yield break;
        }
    }

    IEnumerator AttackCombo1()
    {
        animator.SetInteger("ComboStep", comboStep);

        yield return new WaitForSeconds(attackDelay);

        if (attackCombosList.Count > 1)
        {
            if (attackCombosList[1] == "heavyAttack") // If clicked within time window, continue combo
            {
                comboStep += 1;
                animator.SetTrigger("heavyAttack");
                animator.SetInteger("ComboStep", comboStep);

                yield return new WaitForSeconds(attackDelay);

                if (attackCombosList.Count > 2)
                {
                    if(attackCombosList[2] == "lightAttack")
                    {
                        comboStep += 1;
                        animator.SetTrigger("lightAttack");
                        animator.SetInteger("ComboStep", comboStep);
                    }
                }
                else
                {
                    ResetCombo1();
                    yield break;
                }
            }
            else
            {
                ResetCombo1();
                yield break;
            }
        }
        else
        {
            ResetCombo1();
            yield break;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (cc == null)
            cc = GetComponent<CharacterController>();

        Vector3 bottomCenter = transform.position + cc.center - new Vector3(0, cc.height / 2f, 0);
        Vector3 jumpCheckPosition = bottomCenter + Vector3.up * jumpCheckOffset;
        Vector3 gravityCheckPosition = bottomCenter - Vector3.up * gravityCheckOffset;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(jumpCheckPosition, checkRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gravityCheckPosition, checkRadius);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy Weapon"))
        {
            Debug.Log("Hit by an Enemy Weapon!");
            if (state == HachimanState.Guarding)
            {
                // Sound
                activeKatana.GetComponents<AudioSource>()[0].PlayOneShot(banqueAudio.sSwordAirSwing1);
                // Code
                animator.SetBool("Hit", true);
                Invoke("NotHit", 0.33f);
                DoNotListenToInputs();
                inputActions.MapNormale.Guarding.performed += Guarding;
                inputActions.MapNormale.Guarding.canceled += StopGuarding;
                inputActions.MapNormale.LockOn.performed += LockOn;

                Debug.Log(health);
            }
            else if (state == HachimanState.Rolling)
            {
                Debug.Log(health);
            }
            else 
            {
                if(!isDead)
                {
                    animator.SetBool("Hit", true);
                    health -= 20;
                    Debug.Log(health);
                    if(health <= 0)
                    {
                        Death();
                    }
                    else
                    {
                        Invoke("NotHit", 0.33f);
                        animator.SetFloat("HitVariants", UnityEngine.Random.Range(1, 6));
                        DoNotListenToInputs();
                        inputActions.MapNormale.Guarding.performed += Guarding;
                        inputActions.MapNormale.Guarding.canceled += StopGuarding;
                        inputActions.MapNormale.LockOn.performed += LockOn;  
                    }
                }
            }
        }
    }
    
    /* ===================== FUNCTIONS FOR SOUNDS ===================== */

    public void soundSwordAirSwing1()
    {
        // ---------- Sound ---------
        activeKatana.GetComponent<AudioSource>().PlayOneShot(banqueAudio.sSwordAirSwing1);
    }

    public void DiedUI()
    {
        health += 100;
        quete.AffichageMort();
    }
}