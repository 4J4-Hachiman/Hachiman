using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MasaraiStateAttack : MasaraiBaseState
{
    private readonly Animator mainAnimator;
    private readonly NavMeshAgent mainNavAgent;
    private int waitTime;
    private bool followPlayer;

    public MasaraiStateAttack(MasaraiStateMachine masaraiSM, MasaraiMain main, Animator mainAnimator, NavMeshAgent mainNavAgent) : base(masaraiSM, main)
    {
        this.mainAnimator = mainAnimator;
        this.mainNavAgent = mainNavAgent;
    }

    public override void StateStart(bool init = false)
    {
        waitTime = 2;

        mainNavAgent.updatePosition = false;

        if (main.AttackType == 0)
        {
            Debug.Log("Trigger SIMPLE attack");
            mainAnimator.applyRootMotion = true;
            mainAnimator.SetInteger(main.AnimParamAtkIndex, main.AttackIndex);
            mainAnimator.SetTrigger(main.AnimParamAttackSimple);
            main.LookAtPlayer();
        }
        else
        {
            Debug.Log("Trigger SPECIAL ATTACK");
            mainAnimator.applyRootMotion = false;
            mainAnimator.SetInteger(main.AnimParamAtkIndex, main.AttackIndex);
            mainAnimator.SetTrigger(main.AnimParamAttackSpecial);
            followPlayer = true;
            main.StartCoroutine(ChargeWait());
        }
    }

    public override void StateExit() 
    { 

    }

    public override void StateUpdate() 
    { 
        if(main.DistanceToPlayerSqrtMag() < 25f)
        {
            followPlayer = false;
        }
    }

    public override void StateFixedUpdate() 
    { 
        if (followPlayer)
        {
            main.LookAtPlayer();
        }
    }

    private IEnumerator ChargeWait()
    {
        yield return new WaitForSeconds(waitTime);

        main.LookAtPlayer();
        mainAnimator.applyRootMotion = true;
        mainAnimator.SetTrigger(main.AnimParamAttackSpecialStart);
        yield break;
    }
}