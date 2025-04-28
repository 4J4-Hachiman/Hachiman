using UnityEngine;
using UnityEngine.AI;

public class MasaraiStateAttack : MasaraiBaseState
{
    private readonly Animator mainAnimator;
    private readonly NavMeshAgent mainNavAgent;

    public MasaraiStateAttack(MasaraiStateMachine masaraiSM, MasaraiMain main, Animator mainAnimator, NavMeshAgent mainNavAgent) : base(masaraiSM, main) 
    { 
        this.mainAnimator = mainAnimator;
        this.mainNavAgent = mainNavAgent;
    }

    public override void StateStart(bool init = false) 
    { 
        // Debug.Log("Entered attack state");
        mainAnimator.applyRootMotion = true;
        mainNavAgent.updatePosition = false;
        // mainAnimator.SetInteger(main.AnimParamAttackSimpleIndex, main.AttackSimpleIndex);
        // mainAnimator.SetTrigger(main.AnimParamAttackSimple);\

        mainAnimator.SetInteger(main.AnimParamAttackSpecialIndex, 1);
        mainAnimator.SetTrigger(main.AnimParamAttackSpecial);
        main.LookAtPlayer();
    }

    public override void StateExit() { }
    public override void StateUpdate() { }
    public override void StateFixedUpdate() { }
}
