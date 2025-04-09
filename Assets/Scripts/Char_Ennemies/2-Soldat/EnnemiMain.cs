/* 
    Classe générale de gestion des ennemis

    ********************************************************************
    Par : Yanis Oulmane;
    Derniere modification : 23/03/2025;
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
    public float vitesseDeplacement = 3f;

    /********************* Animator *********************/
    public Animator Animator { get; private set; }
    public int AnimationAttackIndex { get; private set; }

    /******************* State machine *******************/
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyStateAttack StateAttack { get; private set; }
    public EnemyStateSurround StateSurround { get; private set; }
    public EnemyStateCharge StateCharge { get; private set; }
    public EnemyStatePatrol StatePatrol { get; private set; }
    public EnemyStateDead StateDead { get; private set; }

    [field: SerializeField] public GameObject SwordGameObject { get; private set; }
    private CapsuleCollider swordCollider;
    [field: SerializeField] public float LockOffsetThreshold { get; private set; }

    public Vector3[] patrol;
    public event Action<EnnemiMain> OnEnemyDeath;
    public event Action OnAlertAll;

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
        StateMachine = new EnemyStateMachine();
        StateAttack = new EnemyStateAttack(StateMachine, this);
        StateSurround = new EnemyStateSurround(StateMachine, this);
        StateCharge = new EnemyStateCharge(StateMachine, this);
        StatePatrol = new EnemyStatePatrol(StateMachine, this);
        StateDead = new EnemyStateDead(StateMachine, this);
        swordCollider.enabled = false;

        HpCurrent = HpMax;

        Agent.enabled = true;
        enabled = true;

        this.patrol = new Vector3[patrol.Length];
        for (int i = 0; i < this.patrol.Length; i++)
        {
            this.patrol[i] = patrol[i];
        }

        StatePatrol.OnPlayerSpotted += HandlePlayerSpotted;
        StateMachine.Initalize(StatePatrol, true);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player Weapon"))
        {
            Dommage(other.GetComponent<Sword>().GetDammage());
        }
    }

    public void ManageSwordCollider(int state)
    {
        swordCollider.enabled = state == 1;
    }

    public bool GetSpottedPlayer()
    {
        if (Vector3.Angle(transform.forward, Player.transform.position - transform.position) > 70f)
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
            if (hit.transform.gameObject.layer == 7)
            {
                return false;
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

    public void Dommage(float dmgValue)
    {
        Debug.Log($"Enemy took {dmgValue} dammage.");
        HpCurrent -= dmgValue;

        if (HpCurrent <= 0f)
        {
            Gamemanager.Combat.RemoveFromReadyList(this);
            OnEnemyDeath?.Invoke(this);
            StateMachine.SwitchState(StateDead);
        }
    }

    public void Mort()
    {
        Debug.Log($"<color=green>{gameObject.name}</color> is dead!");
    }

    public void SetNavVitesse(float speed)
    {
        Agent.speed = speed;
    }

    public void SetNavDestination(Vector3 position)
    {
        Agent.SetDestination(position);
    }

    public void LookAtPlayer()
    {
        Vector3 dirToPlayer = Player.transform.position - transform.position;
        dirToPlayer.y = 0;
        transform.forward = dirToPlayer;
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

    /// <summary>Triggers an enemy to attack the player.</summary>
    public void TriggerAttack()
    {
        if(IsWithinAttackDistance())
        {
            // Debug.Log($"<color=green>Enemy was already close so triggered attack animation instantly </color>");
            StateMachine.SwitchState(StateAttack);
        }
        else
        {
            // Debug.Log("<color=green>Charging the player.</color>");
            StateMachine.SwitchState(StateCharge);
        }
        
        Gamemanager.Combat.RemoveFromReadyList(this);
        StateAttack.OnAttackEnd += HandleAttackPerformed;
    }

    private void HandleAttackPerformed()
    {
        StateAttack.OnAttackEnd -= HandleAttackPerformed;
        if (StateMachine.currentState != StateDead)
        {
            StateMachine.SwitchState(StateSurround);
        }
    }

    // public string GetRandomAttackType()
    // {
    //     int i = UnityEngine.Random.Range(0, 2);

    //     return i switch
    //     {
    //         0 => "Attack",
    //         1 => "Attack_Combo",
    //         _ => "Attack"
    //     };
    // }

    /// <summary>Checks if an enemy is within a certain range to attack the player.</summary>
    /// <returns>Is enemy close enough to perform an attack.</returns>
    public bool IsWithinAttackDistance()
    {
        return (transform.position - Player.transform.position).sqrMagnitude < 3.5f;
    }
}
