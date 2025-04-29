/*
    Class principale de Masarai
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 29/04/2025; 
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
    public int AnimParamAttackSpecialStart {get; private set; }
    public int AnimParamAtkIndex { get; private set; }


    [Header("Animations")]
    [field: SerializeField] private int simpleAttackCount;
    [field: SerializeField] private int specialAttackCount;
    public int AttackIndex { get; private set; }
    
    [field: SerializeField] public int AttackSpecialTriggerChance { get; private set; }
    public int AttackType { get; private set; }

    /* ================== State Machine ================== */
    private MasaraiStateMachine stateMachine;
    private MasaraiStateWalk stateWalk;
    private MasaraiStateAttack stateAttack;
    private MasaraiStateAttackStart stateAttackStart;

    /* ====================== Events ====================== */

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        capsuleCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        AnimParamVtotal = Animator.StringToHash("Vtotal");
        AnimParamAttackSimple = Animator.StringToHash("AttackSimple");
        AnimParamAttackSpecial = Animator.StringToHash("AttackSpecial");
        AnimParamAtkIndex = Animator.StringToHash("AttackIndex");
        AnimParamAttackSpecialStart = Animator.StringToHash("AttackSpecialStart");

        AtkTrigDistance *= AtkTrigDistance;
        SpecialAtkTrigDistance *= SpecialAtkTrigDistance;

        stateMachine = new MasaraiStateMachine();
        stateWalk = new MasaraiStateWalk(stateMachine, this, animator, agent);
        stateAttack = new MasaraiStateAttack(stateMachine, this, animator, agent);
        stateAttackStart = new MasaraiStateAttackStart(stateMachine, this, animator, agent);

        stateMachine.Initalize(stateWalk);
        stateWalk.OnTriggerAttack += OnTriggerAttack;
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
        agent.nextPosition = transform.position;
    }

    /**************************** STATE EVENTS ****************************/
    private void OnTriggerAttack(int attackType)
    {
        stateWalk.OnTriggerAttack -= OnTriggerAttack;
        AttackType = attackType;

        if (attackType == 0)
        {
            AttackIndex = UnityEngine.Random.Range(0, simpleAttackCount);
            // Debug.Log("Triggered SIMPLE attack");
            stateMachine.SwitchState(stateAttack);
        }
        else
        {
            // Debug.Log("Triggered SPECIAL attack");
            AttackIndex = 1;
            stateMachine.SwitchState(stateAttack);
        }
    }   

    public void LookAtPlayer()
    {
        Vector3 dir = Player.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public float DistanceToPlayerSqrtMag()
    {
        return (Player.position - transform.position).sqrMagnitude;
    }

    /*************************** ANIMATION EVENTS ***************************/
    
    private void OnAttackAnimationEnd()
    {
        stateMachine.SwitchState(stateWalk);
        stateWalk.OnTriggerAttack += OnTriggerAttack;
    }

    private void OnSpecialAttackCharge()
    {
        Debug.Log("Attack charged");
    }
}