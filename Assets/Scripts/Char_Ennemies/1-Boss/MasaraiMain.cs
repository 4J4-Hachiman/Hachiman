/*
    Class principale de Masarai
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/05/2025; 
*/

using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider), typeof(NavMeshAgent))]
public class MasaraiMain : MonoBehaviour
{
    public Transform Player { get; private set; }

    /* =================== Components =================== */
    private CapsuleCollider capsuleCollider;
    private NavMeshAgent agent;
    private Animator animator;

    [field: SerializeField] private Transform[] limbs;
    [field: SerializeField] private Transform hitbox;
    private SphereCollider hitboxCollider;

    [Header("Data")]
    [field: SerializeField] private float maxHp;
    private float currentHp;
    [field: SerializeField] public float AtkTrigDistance { get; private set; }
    [field: SerializeField] public float SpecialAtkTrigDistance { get; private set; }

    /* =================== Anim params =================== */
    public int ParamVtotal { get; private set; }
    public int ParamAttckSpecialStart { get; private set; }
    public int ParamAttackIndex { get; private set; }
    public int ParamAttackSpecialPlay { get; private set; }

    [Header("Healthbar")]
    [field: SerializeField] private GameObject healthbar;
    [field: SerializeField] private Image healthbarFill;
    public int AttackIndex { get; private set; }
    [field: SerializeField] public int AttackSpecialTriggerChance { get; private set; }

    /* ================== State Machine ================== */
    private MasaraiStateMachine stateMachine;
    private MasaraiStateWalk stateWalk;
    private MasaraiStateAttack stateAttack;

    /* ====================== Events ====================== */

    public enum AttackType
    {
        Simple,
        Special
    }

    public enum AttackSimple
    {
        Uppercut,
        Kick,
        Stomp
    }

    public enum AttackSpecial
    {
        Smash,
        Tatsumaki
    }

    public AttackType CurrentAttackType { get; private set; }
    public AttackSimple CurrentSimpleAttack { get; private set; }
    public AttackSpecial CurrentSpecialAttack { get; private set; }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHp = maxHp;

        capsuleCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        ParamVtotal = Animator.StringToHash("Vtotal");
        ParamAttackIndex = Animator.StringToHash("AttackIndex");
        ParamAttckSpecialStart = Animator.StringToHash("AttackSpecialStart");
        ParamAttackSpecialPlay = Animator.StringToHash("AttackSpecialPlay");

        AtkTrigDistance *= AtkTrigDistance;
        SpecialAtkTrigDistance *= SpecialAtkTrigDistance;

        hitboxCollider = hitbox.GetComponent<SphereCollider>();
        hitboxCollider.enabled = false;

        stateMachine = new MasaraiStateMachine();
        stateWalk = new MasaraiStateWalk(stateMachine, this, animator, agent);
        stateAttack = new MasaraiStateAttack(stateMachine, this, animator, agent, hitboxCollider);

        stateMachine.Initalize(stateWalk);
        stateWalk.OnTriggerAttack += OnTriggerAttack;

        healthbar.SetActive(true);
    }
    
    private void Update()
    {
        stateMachine.Current.StateUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.Current.StateFixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player Weapon"))
        {
            if (other.TryGetComponent(out Sword sword))
            {
                currentHp -= sword.GetDammage();
                healthbarFill.fillAmount = currentHp / maxHp;
                if (currentHp < 0)
                {
                    GameEvents.TrigAllEnemiesKilled();
                }
            }
        }
    }

    private void OnAnimatorMove()
    {
        if (!animator.applyRootMotion)
        {
            return;
        }

        transform.position += animator.deltaPosition;
        agent.nextPosition = transform.position;
    }

    /**************************** STATE EVENTS ****************************/
    private void OnTriggerAttack(AttackType type, int forcedAttack)
    {
        stateWalk.OnTriggerAttack -= OnTriggerAttack;
        CurrentAttackType = type;

        if (forcedAttack != -1)
        {
            AttackIndex = forcedAttack;
            stateMachine.SwitchState(stateAttack);
            return;
        }

        if (CurrentAttackType == AttackType.Simple)
        {
            AttackIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(AttackSimple)).Length);
        }
        else
        {
            AttackIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(AttackSpecial)).Length);
        }

        stateMachine.SwitchState(stateAttack);
    }

    public void LookAtPlayer()
    {
        Vector3 dir = Player.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(Player.position, transform.position);
    }

    /*************************** ANIMATION EVENTS ***************************/

    private void OnAttackAnimationEnd()
    {
        stateMachine.SwitchState(stateWalk);
        stateWalk.OnTriggerAttack += OnTriggerAttack;
    }

    private void OnSpecialAttackCharge() { }

    private void OnAttackHitStart(int limbIndex)
    {
        hitbox.parent = limbs[limbIndex];
        hitbox.position = limbs[limbIndex].position;
        hitboxCollider.enabled = true;
    }

    private void OnAttackHitEnd()
    {
        hitboxCollider.enabled = false;
    }
}