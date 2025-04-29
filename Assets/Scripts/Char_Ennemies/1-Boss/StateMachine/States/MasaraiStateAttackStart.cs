using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MasaraiStateAttackStart : MasaraiBaseState
{
    private readonly Animator mainAnimator;
    private readonly NavMeshAgent mainNavAgent;
    private int waitTime;
    public event Action OnSpecialAttackCharged;

    public MasaraiStateAttackStart(MasaraiStateMachine masaraiSM, MasaraiMain main, Animator mainAnimator, NavMeshAgent mainNavAgent) : base(masaraiSM, main) 
    { 
        this.mainAnimator = mainAnimator;
        this.mainNavAgent = mainNavAgent;
    }

    public override void StateStart(bool init = false)
    {
        mainAnimator.applyRootMotion = false;
        mainNavAgent.destination = main.transform.position;
        mainNavAgent.speed = 0;

        waitTime = 3;
        main.StartCoroutine(Wait());
    }

    public override void StateExit() { }
    public override void StateUpdate() { }
    public override void StateFixedUpdate() { }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        mainAnimator.SetTrigger(main.AnimParamAttackSpecial);
    } 
}
