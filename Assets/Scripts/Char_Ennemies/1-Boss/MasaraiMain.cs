/*
    Class principale de Masarai
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 24/04/2025; 
*/

using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider), typeof(NavMeshAgent))]
public class MasaraiMain : MonoBehaviour
{
    public Transform Player { get; private set; }

    /* =================== Components =================== */
    private CapsuleCollider capsuleCollider;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Data")]
    [field: SerializeField] private float hp;
    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float AtkTrigDistance { get; private set; }
    [field: SerializeField] public float SpecialAtkTrigDistance { get; private set; }

    /* =================== Anim params =================== */
    public int AnimParamVtotal { get; private set; }
    public int AnimParamAttackSimple { get; private set; }
    public int AnimParamAttackSpecial { get; private set; }
    public int AnimParamAttackSimpleIndex { get; private set; }
    public int AnimParamAttackSpecialIndex { get; private set; }

    [Header("Animations")]
    [field: SerializeField] private int simpleAttackCount;
    [field: SerializeField] private int specialAttackCount;

    public int AttackSimpleIndex { get; private set; }
    public int AttackSecialIndex { get; private set; }

    /* ================== State Machine ================== */
    private MasaraiStateMachine stateMachine;
    private MasaraiStateWalk stateWalk;
    private MasaraiStateAttack stateAttack;

    /* ====================== Events ====================== */
    public event Action OnAttackAnimEnd;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        capsuleCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        AnimParamVtotal = Animator.StringToHash("Vtotal");
        AnimParamAttackSimple = Animator.StringToHash("AttackSimple");
        AnimParamAttackSpecial = Animator.StringToHash("AttackSpecial");
        AnimParamAttackSimpleIndex = Animator.StringToHash("AttackSimpleIndex");
        AnimParamAttackSpecialIndex = Animator.StringToHash("AttackSpecialIndex");

        stateMachine = new MasaraiStateMachine();
        stateWalk = new MasaraiStateWalk(stateMachine, this, animator, agent);
        stateAttack = new MasaraiStateAttack(stateMachine, this, animator, agent);

        stateMachine.Initalize(stateWalk);
        stateWalk.OnCloseToPLayer += OnCloseToPlayer;
    }

    private void Update()
    {
        stateMachine.Current.StateUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.Current.StateFixedUpdate();
    }

    private void OnAnimatorMove()
    {
        if (!animator.applyRootMotion)
        {
            return;
        }
        
        transform.position += animator.deltaPosition;
    }

    private void OnCloseToPlayer()
    {
        stateWalk.OnCloseToPLayer -= OnCloseToPlayer;
        AttackSimpleIndex = UnityEngine.Random.Range(0, simpleAttackCount);
        stateMachine.SwitchState(stateAttack);
    }

    public void OnAttackAnimationEnd()
    {
        Debug.Log("Attack Animation over");
        stateMachine.SwitchState(stateWalk);
        stateWalk.OnCloseToPLayer += OnCloseToPlayer;
    }

    public void LookAtPlayer()
    {
        Vector3 dir = Player.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}