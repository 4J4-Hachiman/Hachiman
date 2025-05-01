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
    public ControlesVieMana uiVieMana;
    public ParticleSystem sparksBlockEffect;

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
    private bool canMove;
    private bool isDead;
    private bool isGrounded;
    private bool isHealing;
    private bool isHit;
    private bool canHitRock = true;

    //Rock Quest
    private bool inFrontofRock = false;

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
    private int lockOnIndex = 0;
    private int lockOnTotalTargets = 0;

    // Rock Quest
    private int rockIndex = 0;

    public float health = 100;
    public float maxHealth = 100;
    public float endurance = 100;
    public float maxEndurance = 100;
    public int numbPotion = 3;
    public float enduranceRegen = 1;

    /* -------------------- VARIABLES GAMEOBJECT -------------------- */
    public GameObject activeKatana;
    public GameObject katana;
    public GameObject katanaInSheath;
    public GameObject healthPotion;
    public GameObject camera;
    public Transform head;
    private Transform lockOnTarget;
    public GameObject debugTool;
    public RectTransform lockOnDot;
    public GameObject dotCanvas;
    private GameObject currentRock;
    private GameObject nextRock;
    [SerializeField] private GameObject uiInteractionRock;

    /* --------------------------- ARRAYS ---------------------------- */ 
    private Collider[] hits;
    [SerializeField] private GameObject[] rocks;

    /* ============================================================== */
    /* ============================================================== */

    // -----------------------------------------------------------************************************************************************ Sebastien//
    public enum HachimanState{
        Idle, 
        Equiping,
        Healing,
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
        isHealing = false;
        isHit = false;

        comboStep = Mathf.Clamp(comboStep, 0, 3);
        lightAttackCombo = "lightAttack";
        heavyAttackCombo = "heavyAttack";
        
        inputActions = new PlayerControls();
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        forceGravite = -Math.Abs(forceGravite);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputMouvement = inputActions.MapNormale.Mouvement;
        inputActions.MapNormale.Crouch.performed += Crouch;
        inputActions.MapNormale.Roll.performed += Roll;
        inputActions.MapNormale.LightAttack.performed += LightAttack;
        inputActions.MapNormale.HeavyAttack.performed += HeavyAttack;
        inputActions.MapNormale.Unsheath.performed += Unsheath;
        inputActions.MapNormale.LockOn.performed += LockOn;
        inputActions.MapNormale.Guarding.performed += Guarding;
        inputActions.MapNormale.Guarding.canceled += StopGuarding;
        inputActions.MapNormale.Heal.performed += Heal;
        inputActions.MapNormale.LockOnIndexR2.performed += LockOnIndexR2;
        inputActions.MapNormale.DebugTool.performed += DebugTool;
        inputActions.MapNormale.Interact.performed += Interact;

    }

    private void OnDisable()
    {
        // Desactiver le ActionMap et se desabonner aux evenements
        inputActions.Disable();
        inputActions.MapNormale.Crouch.performed -= Crouch;
        inputActions.MapNormale.Roll.performed -= Roll;
        inputActions.MapNormale.LightAttack.performed -= LightAttack;
        inputActions.MapNormale.HeavyAttack.performed -= HeavyAttack;
        inputActions.MapNormale.Unsheath.performed -= Unsheath;
        inputActions.MapNormale.LockOn.performed -= LockOn;
        inputActions.MapNormale.Guarding.performed -= Guarding;
        inputActions.MapNormale.Guarding.canceled -= StopGuarding;
        inputActions.MapNormale.Heal.performed -= Heal;
        inputActions.MapNormale.LockOnIndexR2.performed -= LockOnIndexR2;
        inputActions.MapNormale.DebugTool.performed -= DebugTool;
        inputActions.MapNormale.Interact.performed -= Interact;
    }


    /* ================================ METHODES UPDATE ================================ */

    void Update()
    {
        //Debug Log Update
        
        // Debug.Log("<color=red>Health: </color>" + health);
        // Debug.Log("<color=Green>Endurance: </color>" + endurance);
        // Debug.Log("<color=Blue>Lock On Index: </color>" + lockOnIndex);
        // Debug.Log("<color=Purple>Lock On Total Targets: </color>" + lockOnTotalTargets);

        //Debug.DrawRay(head.position, Vector3.up * checkDistance, Color.red);
        //Debug.Log("attackCombosList Count: " + attackCombosList.Count);
        //Debug.Log("comboStep: " + comboStep);
        //Debug.Log("isCombo: " + isCombo);

        //Death

        Debug.Log("<color=Blue>State: </color>" + state);

        if (health <= 0)
        {
            isLockedOn = false;
            animator.SetBool("lockedOn", false);
        }

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

        uiVieMana.AffichageNiveauMana(maxEndurance, endurance);
        uiVieMana.AffichageNiveauVie(maxHealth, health);
        uiVieMana.QuantitePotionVie(numbPotion);


        if (isLockedOn == true)
        {
            if (lockOnTarget == null)
            {
                isLockedOn = false;
                animator.SetBool("lockedOn", false);
            } 
            else 
            {
                dotCanvas.SetActive(true);
                Vector3 direction = lockOnTarget.position - transform.position;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);
                //UI dot
                lockOnDot.position = lockOnTarget.position + (Vector3.up * 1f);
                lockOnDot.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
            }  
        }
        else
        {
            dotCanvas.SetActive(false);
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

    void LockOnFunc()
    {
        lockOnTarget = null;
        lockOnIndex = 0;
        hits = CapsuleCastFromCamera();
        lockOnTotalTargets = hits.Length;

        if (hits.Length > 0 && hits[lockOnIndex].transform != null)
        {
            lockOnTarget = hits[lockOnIndex].transform;
        }
        if (lockOnTarget == null)
        {
            isLockedOn = false;
            animator.SetBool("lockedOn", false);
        }
    }

    /////////////////////////////////////////////////////////////////////
    // FUNCTIONS ////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////
    
    public void ResetHachiman()
    {
        Debug.Log("<color=Green> RESET </color>");
        state = HachimanState.Idle;
        isLockedOn = false;
        isArmed = false;
        isJumping = false;
        isCrouched = false;
        isRolling = false;
        isDead = false;
        canJump = true;
        isHealing = false;
        isHit = false;

        comboStep = 0;
        combo2Step = 0;

        inputActions.Enable();
        inputMouvement = inputActions.MapNormale.Mouvement;
        inputActions.MapNormale.Crouch.performed += Crouch;
        inputActions.MapNormale.Roll.performed += Roll;
        inputActions.MapNormale.LightAttack.performed += LightAttack;
        inputActions.MapNormale.HeavyAttack.performed += HeavyAttack;
        inputActions.MapNormale.Unsheath.performed += Unsheath;
        inputActions.MapNormale.LockOn.performed += LockOn;
        inputActions.MapNormale.Guarding.performed += Guarding;
        inputActions.MapNormale.Guarding.canceled += StopGuarding;
        inputActions.MapNormale.Heal.performed += Heal;
        inputActions.MapNormale.LockOnIndexR2.performed += LockOnIndexR2;
        inputActions.MapNormale.DebugTool.performed += DebugTool;
        inputActions.MapNormale.Interact.performed += Interact;
    }

    public void TPlocation1()
    {
        Vector3 teleportLocation = new Vector3(150f, 0.1f, 70.8f);
        cc.enabled = false; // disable the controller first
        transform.position = teleportLocation; // teleport!
        cc.enabled = true; // re-enable the controller
    }

    public void TPlocation2()
    {
        Vector3 teleportLocation = new Vector3(167.9f, 0.1f, 136.2f);
        cc.enabled = false; // disable the controller first
        transform.position = teleportLocation; // teleport!
        cc.enabled = true; // re-enable the controller
    }

    public void TPlocation3()
    {
        Vector3 teleportLocation = new Vector3(122.2f, 0.1f, 204f);
        cc.enabled = false; // disable the controller first
        transform.position = teleportLocation; // teleport!
        cc.enabled = true; // re-enable the controller
    }

    public void PauseGame()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

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
            // Debug.Log(col.gameObject);
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
                //Debug.Log("Cone hit: " + col.name);
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
        isHit = false;
        animator.SetBool("Hit", false);
        ListenToInputs();
    }

    void NotHitBroken()
    {
        isHit = false;
        ListenToInputs();
        state = HachimanState.Idle;
        animator.SetBool("guarding", false);    
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
    public void ResetState()
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

            default: canMove = false;
            break;
        }
    }
    private void DoNotListenToInputs()
    {
        inputActions.MapNormale.Crouch.performed -= Crouch;
        inputActions.MapNormale.Roll.performed -= Roll;
        inputActions.MapNormale.LightAttack.performed -= LightAttack;
        inputActions.MapNormale.HeavyAttack.performed -= HeavyAttack;
        inputActions.MapNormale.Unsheath.performed -= Unsheath;
        inputActions.MapNormale.LockOn.performed -= LockOn;
        inputActions.MapNormale.Guarding.performed -= Guarding;
        inputActions.MapNormale.Guarding.canceled -= StopGuarding;
        inputActions.MapNormale.Heal.performed -= Heal;
        inputActions.MapNormale.Interact.performed -= Interact;
    }
    private void ListenToInputs()
    {
        inputActions.MapNormale.Crouch.performed += Crouch;
        inputActions.MapNormale.Roll.performed += Roll;
        inputActions.MapNormale.LightAttack.performed += LightAttack;
        inputActions.MapNormale.HeavyAttack.performed += HeavyAttack;
        inputActions.MapNormale.Unsheath.performed += Unsheath;
        inputActions.MapNormale.LockOn.performed += LockOn;
        inputActions.MapNormale.Guarding.performed += Guarding;
        inputActions.MapNormale.Guarding.canceled += StopGuarding;
        inputActions.MapNormale.Heal.performed += Heal;
        inputActions.MapNormale.Interact.performed += Interact;
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

    private void Interact(InputAction.CallbackContext ctx)
    {
        if(ctx.performed){
            if(inFrontofRock && canHitRock && isArmed)
            {
                for (int i = 0; i < rocks.Length - 1; i++)
                {
                    Vector3 direction = rocks[i].transform.position - transform.position;
                    direction.y = 0;
                    transform.rotation = Quaternion.LookRotation(direction);

                    currentRock = rocks[i];
                    nextRock = rocks[i + 1];

                    if(currentRock.activeSelf)
                    {
                        canHitRock = false;
                        animator.SetTrigger("HitRock");
                        StartCoroutine(RockBreaking());
                        break;
                    }
                }
            }
        }
    }
    private void DebugTool(InputAction.CallbackContext ctx)
    {
        if (ctx.performed){
            if (!debugTool.activeSelf)
            {
                debugTool.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                debugTool.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    private void Heal(InputAction.CallbackContext ctx)
    {
        if (ctx.performed){
            if(numbPotion >= 1)
            {
                //state = HachimanState.Healing;
                Debug.Log("Heal");
                if (isHealing == false){
                    healthPotion.SetActive(true);
                    isHealing = true;
                    animator.SetTrigger("Healing");
                    Invoke("soundHealing", 0.6f);
                    DoNotListenToInputs();
                    health = (health > 51) ? 100 : health + 50;
                    numbPotion -= 1;
                    animator.SetLayerWeight(1, 1);
                    Invoke("StoppedHealing", 1.83f);
                } 
            }
        }
    }

    public void StoppedHealing()
    {
        healthPotion.SetActive(false);
        ListenToInputs();
        isHealing = false;
        animator.SetLayerWeight(1, 0);
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
                    GetComponents<AudioSource>()[6].PlayOneShot(banqueAudio.sCrouch);
                } else {
                    ListenToInputs();
                    isCrouched = false;
                    animator.SetBool("crouch", false);
                    GetComponents<AudioSource>()[6].PlayOneShot(banqueAudio.sCrouch);
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
        }
    }

    // -----------------------------------------------------------************************************************************************ Sebastien//
    private void LightAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isArmed){
            // ---------- State ---------
            DoNotListenToInputs();
            animator.SetTrigger("lightAttack");
            // Code
            inputActions.MapNormale.LightAttack.performed += LightAttack;
            inputActions.MapNormale.HeavyAttack.performed += HeavyAttack;
            if (attackCombosList.Count < maxCombos)
            {
                attackCombosList.Add("lightAttack");
            }
            
            if (isCombo == false)
            {
                state = HachimanState.Attacking;
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
            DoNotListenToInputs();
            animator.SetTrigger("heavyAttack");
            inputActions.MapNormale.LightAttack.performed += LightAttack;
            inputActions.MapNormale.HeavyAttack.performed += HeavyAttack;
            if (attackCombosList.Count < maxCombos2)
            {
                attackCombosList.Add("heavyAttack");
            }
            if (isCombo == false)
            {
                state = HachimanState.Attacking;
                isCombo = true;
                combo2Step += 1;
                StartCoroutine(AttackCombo2());
            }
        }
    }

    // -----------------------------------------------------------************************************************************************ Sebastien//
    private void Unsheath(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && state != HachimanState.Equiping){
            state = HachimanState.Equiping;
            //Debug.Log(ctx);
            if (isArmed == false){
                Debug.Log("armed");
                isArmed = true;
                animator.SetBool("armed", true);
                GetComponents<AudioSource>()[5].PlayOneShot(banqueAudio.sUnsheath);
                Invoke("UnsheathKatana", 0.17f);
                
            } else {
                isArmed = false;
                animator.SetBool("armed", false);
                Invoke("SheathKatanaSound", 0.6f);
                Invoke("SheathKatana", 1.22f);
            }
        }
    }

    void SheathKatana()
    {
        activeKatana.gameObject.SetActive(false);
        katanaInSheath.gameObject.SetActive(true);
    }

    void SheathKatanaSound()
    {
        GetComponents<AudioSource>()[4].PlayOneShot(banqueAudio.sSheath);
    }

    void UnsheathKatana()
    {
        activeKatana.gameObject.SetActive(true);
        katanaInSheath.gameObject.SetActive(false);
    }

    private void LockOn(InputAction.CallbackContext ctx)
    {
        //Debug.Log(ctx);
        if (ctx.performed){
            if (isLockedOn == false){
                LockOnFunc();
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
            if(endurance > 0)
            {
                state = HachimanState.Guarding;
                animator.SetBool("guarding", true);
                inputActions.MapNormale.Crouch.performed -= Crouch;
                inputActions.MapNormale.LightAttack.performed -= LightAttack;
                inputActions.MapNormale.HeavyAttack.performed -= HeavyAttack;
                inputActions.MapNormale.Unsheath.performed -= Unsheath;
                inputActions.MapNormale.LockOn.performed -= LockOn;
            }
        }
    }
    private void StopGuarding(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled && isArmed){
            state = HachimanState.Idle;
            animator.SetBool("guarding", false);
            animator.SetBool("Hit", false);
            ListenToInputs();
        }
    }

    private void LockOnIndexR2(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isLockedOn == true){
            if(lockOnIndex < (lockOnTotalTargets-1))
            {
                lockOnIndex += 1;
                lockOnTarget = hits[lockOnIndex].transform;
            }
            else
            {
                lockOnIndex = 0;
                lockOnTarget = hits[lockOnIndex].transform;
            }
        }
    }


    /* ===================== METHODES POUR LE DÉPLACEMENT DU JOUEUR ===================== */

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

    IEnumerator GestionGravite()
    {
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
            else
            {
                ResetCombo2();
                yield break;
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

    IEnumerator EnduranceReset()
    {
        Debug.Log("<color=yellow>Blocked</color>");
        yield return new WaitForSeconds(3f);

        while(endurance <= 100)
        {
            if (isHit)
            {
                StartCoroutine(EnduranceReset());
                yield break;
            }
            endurance += enduranceRegen * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    IEnumerator RockBreaking()
    {
        yield return new WaitForSeconds(0.4f);

        currentRock.SetActive(false);
        nextRock.SetActive(true);
        
        if(nextRock.name == "Roche4")
        {
            GetComponents<AudioSource>()[3].PlayOneShot(banqueAudio.sRockBreak);
            uiInteractionRock.SetActive(false);
        }
        else
        {
            GetComponents<AudioSource>()[2].PlayOneShot(banqueAudio.sRockHit);
        }

        yield return new WaitForSeconds(1f);

        canHitRock = true;

        yield break;
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
            if (state == HachimanState.Rolling)
            {
                Debug.Log(health);
            }
            else
            {
                
            // Vector3 direction = lockOnTarget.position - transform.position;
            // direction.y = 0;
            // transform.rotation = Quaternion.LookRotation(direction);

            Debug.Log("Hit by an Enemy Weapon!");
            if (state == HachimanState.Guarding)
            {
                endurance = (endurance < 25) ? 0 : endurance - 25;
                isHit = true;
                StartCoroutine(EnduranceReset());
                DoNotListenToInputs();
                inputActions.MapNormale.Guarding.performed += Guarding;
                inputActions.MapNormale.Guarding.canceled += StopGuarding;
                inputActions.MapNormale.LockOn.performed += LockOn;
                if(endurance <= 0)
                {
                    Invoke("NotHitBroken", 0.33f);
                    animator.SetTrigger("Broken");
                    ParticleSystem sparksInt = Instantiate(sparksBlockEffect, katana.transform.position, katana.transform.rotation);
                    Destroy(sparksInt.gameObject, 2f);
                    activeKatana.GetComponents<AudioSource>()[1].PlayOneShot(banqueAudio.sStanceBroken);
                }
                else
                {
                    Invoke("NotHit", 0.33f);
                    animator.SetBool("Hit", true);
                    ParticleSystem sparksInt = Instantiate(sparksBlockEffect, katana.transform.position, katana.transform.rotation);
                    Destroy(sparksInt.gameObject, 2f);
                    activeKatana.GetComponents<AudioSource>()[2].PlayOneShot(banqueAudio.sSwordClash2);
                }
            }
            else 
            {
                if(!isDead)
                {
                    animator.SetBool("Hit", true);
                    GetComponents<AudioSource>()[0].PlayOneShot(banqueAudio.sEnemyHit2);
                    health -= 20f;
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
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Rock"))
        {
            inFrontofRock = true;
        }
    }
    
    /* ===================== FUNCTIONS FOR SOUNDS ===================== */

    public void soundSwordAirSwing3()
    {
        // ---------- Sound ---------
        activeKatana.GetComponents<AudioSource>()[0].PlayOneShot(banqueAudio.sSwordAirSwing3);
    }
    public void soundSwordAirSwing1()
    {
        // ---------- Sound ---------
        activeKatana.GetComponents<AudioSource>()[3].PlayOneShot(banqueAudio.sSwordAirSwing1);
    }
    public void soundSwordAirSwing2()
    {
        // ---------- Sound ---------
        activeKatana.GetComponents<AudioSource>()[4].PlayOneShot(banqueAudio.sSwordAirSwing2);
    }
    
    public void soundHealing()
    {
        // ---------- Sound ---------
        GetComponents<AudioSource>()[1].PlayOneShot(banqueAudio.sHealing);
    }
}