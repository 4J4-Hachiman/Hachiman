using System;
using UnityEngine;
using UnityEngine.AI;

public class MasaraiStateWalk : MasaraiBaseState
{
    public event Action OnCloseToPLayer;
    private readonly Animator mainAnimator;
    private readonly NavMeshAgent mainNavAgent;

    public MasaraiStateWalk(MasaraiStateMachine masaraiSM, MasaraiMain main, Animator mainAnimator, NavMeshAgent mainNavAgent) : base(masaraiSM, main) 
    { 
        this.mainAnimator = mainAnimator;
        this.mainNavAgent = mainNavAgent;
    }

    public override void StateStart(bool init = false) 
    { 
        Debug.Log("Entered walk state");
        mainAnimator.applyRootMotion = true;
        mainNavAgent.updatePosition = false;
    }

    public override void StateExit() { }

    public override void StateUpdate() 
    { 
        if ((main.Player.position - main.transform.position).sqrMagnitude < main.AtkTrigDistance)
        {
            OnCloseToPLayer?.Invoke();
        }
        
        mainNavAgent.nextPosition = main.transform.position;
        mainAnimator.SetFloat(main.AnimParamVtotal, mainNavAgent.velocity.magnitude);
    }

    public override void StateFixedUpdate()
    {
        mainNavAgent.SetDestination(main.Player.position);
        main.LookAtPlayer();
    }
}