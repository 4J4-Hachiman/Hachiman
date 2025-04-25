/*
    Class principale de Masarai
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 24/04/2025; 
*/

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

    /* =================== Anim params =================== */
    private int animParamVtotal;

    /* ================== State Machine ================== */
    private MasaraiStateMachine stateMachine;
    private MasaraiStateWalk stateWalk;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        capsuleCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animParamVtotal = Animator.StringToHash("Vtotal");

        stateMachine = new MasaraiStateMachine();
        stateWalk = new MasaraiStateWalk(stateMachine, this);

        stateMachine.Initalize(stateWalk);
    }

    private void Update()
    {
        stateMachine.Current.StateUpdate();
    }

    private void FixedUpdate()
    {
        animator.SetFloat(animParamVtotal, agent.velocity.magnitude);
        stateMachine.Current.StateFixedUpdate();
    }

    public void SetNavAgentDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
