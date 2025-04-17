/* 
    Classe générale de gestion des ennemis

    ********************************************************************
    Par : Yanis Oulmane;
    Derniere modification : 12/04/2025;
*/

using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(
    typeof(NavMeshAgent),
    typeof(CapsuleCollider),
    typeof(Animator)
)]

public class EnnemiMain : MonoBehaviour, IDamageable, IMoveable
{
    /* ======================== VARIABLES ======================== */

    /* ----  IDamageable Interface ---- */
    [field: SerializeField] public float HpMax { get; set; }
    public float HpCurrent { get; set; }
    public CapsuleCollider CapsuleCollider { get; set; }

    /* ------ IMovable Interface ------ */
    public NavMeshAgent Agent { get; set; }

    /* ------- Class variables ------- */
    public GameObject Player { get; private set; }
    public Gamemanager Gamemanager { get; private set; }

    public bool IsAttacking { get; private set; } = false;
    public float vitesseDeplacement = 3;

    /********************* Animator *********************/
    public Animator Animator { get; private set; }
    public int AnimationAttackIndex { get; private set; }

    /* ==================== STATE MACHINE ==================== */
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyStateAttack StateAttack { get; private set; }
    public EnemyStateSurround StateSurround { get; private set; }
    public EnemyStateCharge StateCharge { get; private set; }
    public EnemyStatePatrol StatePatrol { get; private set; }
    public EnemyStateDead StateDead { get; private set; }
    public EnemyStateHit StateHit { get; private set; }

    [field: SerializeField] public GameObject SwordGameObject { get; private set; }
    private CapsuleCollider swordCollider;
    [field: SerializeField] public float LockOffsetThreshold { get; private set; }

    public Vector3[] patrol;
    public event Action OnAlertAll;
    public event Action<EnnemiMain> OnEnemyDeath;
    public event Action<bool> OnComboStepStart;
    public event Action OnAnimationEnd;
    public event Action<EnnemiMain> OnActionOver;
    public event Action<float, float> OnDammageTaken;

    public enum AttackTypes
    {
        AttackSimple,
        Combo
    }

    /* ======================= END OF VARIABLES ======================= */

    private void Awake()
    {
        LockOffsetThreshold *= LockOffsetThreshold;
        Player = GameObject.FindWithTag("Player");
        swordCollider = SwordGameObject.GetComponent<CapsuleCollider>();
        Agent = GetComponent<NavMeshAgent>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        Animator = GetComponent<Animator>();
        Agent.speed = vitesseDeplacement;
        Gamemanager = GameObject.FindWithTag("GameController").GetComponent<Gamemanager>();
    }

    public void Initialize(Vector3[] patrol)
    {
        StateMachine = new();
        StateAttack = new(StateMachine, this);
        StateSurround = new(StateMachine, this);
        StateCharge = new(StateMachine, this);
        StatePatrol = new(StateMachine, this);
        StateDead = new(StateMachine, this);
        StateHit = new(StateMachine, this);

        swordCollider.enabled = false;
        CapsuleCollider.enabled = true;

        HpCurrent = HpMax;

        Agent.enabled = true;
        Animator.applyRootMotion = false;
        Animator.SetBool("Dead", false);
        enabled = true;

        this.patrol = new Vector3[patrol.Length];
        for (int i = 0; i < this.patrol.Length; i++)
        {
            this.patrol[i] = patrol[i];
        }

        StatePatrol.OnPlayerSpotted += HandlePlayerSpotted;
        StateMachine.Initalize(StatePatrol, true);

        // StartCoroutine(DebugCoroutine());
    }

    // IEnumerator DebugCoroutine()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSecondsRealtime(1);
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player Weapon"))
        {
            // Debug.Log("<color=red>Was hit by the player</color>");
            Dommage(other.GetComponent<Sword>().GetDammage());
        }
    }

    public void Dommage(float dmgValue)
    {
        HpCurrent -= dmgValue;

        if (HpCurrent <= 0f)
        {
            OnEnemyDeath?.Invoke(this);
            Animator.SetTrigger("Dead");
            StateMachine.SwitchState(StateDead);
        }
        else
        {
            StateMachine.SwitchState(StateHit);
            StateHit.OnHitAnimationEnd += HandleStateAnimationEnd;
            OnDammageTaken?.Invoke(HpCurrent, HpMax);
        }

        // Gamemanager.Combat.RemoveFromReadyList(this);
        StateAttack.OnAttackEnd -= HandleOnAttackEnd;
        OnActionOver?.Invoke(this);
    }

    private void HandleStateAnimationEnd()
    {
        StateHit.OnHitAnimationEnd -= HandleStateAnimationEnd;
        StateMachine.SwitchState(StateSurround);
    }

    private void OnAnimatorMove()
    {
        if (StateMachine.currentState != StateAttack)
        {
            return;
        }

        if ((Player.transform.position - transform.position).sqrMagnitude > 1)
        {
            transform.position += Animator.deltaPosition;
        }
    }

    public void ManageSwordCollider(int state)
    {
        swordCollider.enabled = state == 1;
    }

    public bool GetSpottedPlayer()
    {
        if (Vector3.Angle(transform.forward, Player.transform.position - transform.position) > 80)
        {
            return false;
        }

        Ray ray = new(transform.position + (Vector3.up * 1.5f), Player.transform.position - transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f, 136))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
            if (hit.transform.gameObject.layer == 3)
            {
                return true;
            }
        }
        return false;
    }

    public void StartCombat()
    {
        StatePatrol.OnPlayerSpotted -= HandlePlayerSpotted;
        StateMachine.SwitchState(StateSurround, true);
    }

    public void HandlePlayerSpotted()
    {
        OnAlertAll?.Invoke();
    }

    public void SetNavVitesse(float speed)
    {
        Agent.speed = speed;
    }

    public void SetNavDestination(Vector3 position)
    {
        Agent.SetDestination(position);
    }

    public void LookAtPlayer(float speed = 50)
    {
        Vector3 dirToPlayer = Player.transform.position - transform.position;
        dirToPlayer.y = 0;
        transform.forward = Vector3.Slerp(transform.forward, dirToPlayer, speed * Time.deltaTime);
    }

    private Vector2 AgentDir()
    {
        Vector3 vFoward = transform.forward;
        Vector3 vRight = transform.right;
        Vector3 vAgent = Agent.velocity;
        vAgent.y = 0;
        float v2x = Vector3.Dot(vAgent, vRight);
        float v2y = Vector3.Dot(vAgent, vFoward);
        Vector2 rDir = new(v2x, v2y);
        return rDir;
    }

    protected void SetAnimVParams()
    {
        Vector2 v = AgentDir();
        Animator.SetFloat("Vtotal", v.magnitude);
        Animator.SetFloat("Vx", v.normalized.x);
        Animator.SetFloat("Vy", v.normalized.y);
    }

    public void TriggerAttack()
    {
        // Gamemanager.Combat.RemoveFromReadyList(this);
        // Debug.Log("Triggered Attack");
        StateMachine.SwitchState(IsWithinAttackDistance() ? StateAttack : StateCharge);
        StateAttack.OnAttackEnd += HandleOnAttackEnd;
    }

    private void HandleOnAttackEnd()
    {
        OnActionOver?.Invoke(this);
        // Debug.Log("<color=green>Action performed");
        StateAttack.OnAttackEnd -= HandleOnAttackEnd;
        StateMachine.SwitchState(StateSurround);
        // Gamemanager.Combat.AddToReadyList(this);
    }

    public AttackTypes GetRandomAttackType()
    {
        return (AttackTypes)Enum.GetValues(typeof(AttackTypes)).GetValue(UnityEngine.Random.Range(0, Enum.GetValues(typeof(AttackTypes)).Length));
    }

    public bool IsWithinAttackDistance()
    {
        return (transform.position - Player.transform.position).sqrMagnitude <= 2f;
    }

    /* =========================== ANIMATION EVENTS METHODS =========================== */
    private void TriggerOnAnimationEnd()
    {
        // Debug.Log("Animation is over");
        OnAnimationEnd?.Invoke();
    }

    private void TriggerOnComboStart(int lastComboStep = 0)
    {
        if ((lastComboStep & ~1) != 0)
        {
            lastComboStep = 1;
        }

        if (!IsWithinAttackDistance())
        {
            OnComboStepStart?.Invoke(false);
            return;
        }

        if (lastComboStep == 1)
        {
            OnComboStepStart?.Invoke(false);
            return;
        }

        bool performNext = UnityEngine.Random.Range(0, 100) < 65;
        OnComboStepStart?.Invoke(performNext);
    }
}