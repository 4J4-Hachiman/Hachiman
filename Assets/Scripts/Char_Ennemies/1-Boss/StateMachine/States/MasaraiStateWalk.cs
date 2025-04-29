using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MasaraiStateWalk : MasaraiBaseState
{
    // public event Action OnCloseToPLayer;
    public event Action<int> OnTriggerAttack;
    private readonly Animator mainAnimator;
    private readonly NavMeshAgent mainNavAgent;
    private bool attackedTriggered;

    public MasaraiStateWalk(MasaraiStateMachine masaraiSM, MasaraiMain main, Animator mainAnimator, NavMeshAgent mainNavAgent) : base(masaraiSM, main) 
    { 
        this.mainAnimator = mainAnimator;
        this.mainNavAgent = mainNavAgent;
    }

    public override void StateStart(bool init = false) 
    { 
        // Debug.Log("Entered walk state");
        mainAnimator.applyRootMotion = true;
        mainNavAgent.updatePosition = false;
        attackedTriggered = false;
        main.StartCoroutine(SpecialRand());
    }

    public override void StateExit() { }

    public override void StateUpdate() 
    { 
        if ((main.Player.position - main.transform.position).sqrMagnitude < main.AtkTrigDistance)
        {
            OnTriggerAttack?.Invoke(0);
            attackedTriggered = true;
        }
        
        mainNavAgent.nextPosition = main.transform.position;
        mainAnimator.SetFloat(main.AnimParamVtotal, mainNavAgent.velocity.magnitude);
    }

    public override void StateFixedUpdate()
    {
        mainNavAgent.SetDestination(main.Player.position);
        main.LookAtPlayer();
    }

    private IEnumerator SpecialRand()
    {
        float interval = 1;

        while (!attackedTriggered)
        {
            interval -= 1 * Time.deltaTime;
            
            if ((main.Player.position - main.transform.position).sqrMagnitude < main.SpecialAtkTrigDistance && interval < 0)
            {
                // Debug.Log("<color=orange>Checking for special attack trigger</color>");
                // bool trig = UnityEngine.Random.Range(0, 100) < main.AttackSpecialTriggerChance; 
                if (UnityEngine.Random.Range(0, 100) < main.AttackSpecialTriggerChance)
                {
                    // Debug.Log("Triggering Special Attack");
                    attackedTriggered = true;
                    OnTriggerAttack?.Invoke(1);
                    yield break;
                }

                interval = 1;
            }
            yield return null;
        }
        // Debug.Log("Stopped special attack coroutine");
        yield break;
    }
}