using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MasaraiStateWalk : MasaraiBaseState
{
    // public event Action OnCloseToPLayer;
    public event Action<MasaraiMain.AttackType, int> OnTriggerAttack;
    private readonly Animator mainAnimator;
    private readonly NavMeshAgent mainNavAgent;
    private bool attackedTriggered;
    private float timeout;

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
            OnTriggerAttack?.Invoke(MasaraiMain.AttackType.Simple, -1);
            attackedTriggered = true;
        }

        mainNavAgent.nextPosition = main.transform.position;
        mainAnimator.SetFloat(main.ParamVtotal, mainNavAgent.velocity.magnitude);
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
            if (interval > 0)
            {
                yield return null;
            }

            interval = 1;

            if ((main.Player.position - main.transform.position).sqrMagnitude > main.SpecialAtkTrigDistance)
            {
                yield return null;
            }

            if (UnityEngine.Random.Range(0, 100) < main.AttackSpecialTriggerChance)
            {
                attackedTriggered = true;

                if ((main.Player.position - main.transform.position).sqrMagnitude < 26f)
                {
                    OnTriggerAttack?.Invoke(MasaraiMain.AttackType.Special, -1);
                    yield break;
                }

                OnTriggerAttack?.Invoke(MasaraiMain.AttackType.Special, (int)MasaraiMain.AttackSpecial.Smash);
                yield break;
            }

            yield return null;
        }
        yield break;
    }
}