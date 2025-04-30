using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MasaraiStateAttack : MasaraiBaseState
{
    private readonly Animator mainAnimator;
    private readonly NavMeshAgent mainNavAgent;
    private float waitTime;
    private float distanceToPlayer;
    // private bool followPlayer;

    private readonly Dictionary<(int, int), Action> attacks;

    public MasaraiStateAttack(MasaraiStateMachine masaraiSM, MasaraiMain main, Animator mainAnimator, NavMeshAgent mainNavAgent) : base(masaraiSM, main)
    {
        this.mainAnimator = mainAnimator;
        this.mainNavAgent = mainNavAgent;

        attacks = new Dictionary<(int, int), Action>
        {
            {((int)MasaraiMain.AttackType.Simple, (int)MasaraiMain.AttackSimple.Uppercut), () => SimpleUppercut()},
            {((int)MasaraiMain.AttackType.Simple, (int)MasaraiMain.AttackSimple.Kick), () => SimpleKick()},
            {((int)MasaraiMain.AttackType.Simple, (int)MasaraiMain.AttackSimple.Stomp), () => SimpleStomp()},
            {((int)MasaraiMain.AttackType.Special, (int)MasaraiMain.AttackSpecial.Smash), () => main.StartCoroutine(SpecialSmash())},
            {((int)MasaraiMain.AttackType.Special, (int)MasaraiMain.AttackSpecial.Tatsumaki), () => main.StartCoroutine(SpecialTatsumaki())},
        };
    }

    public override void StateStart(bool init = false)
    {
        waitTime = 2;
        mainNavAgent.updatePosition = false;

        // mainAnimator.SetInteger("AttackType", (int)main.CurrentAttackType);
        // mainAnimator.SetInteger("AttackIndex", main.AttackIndex);
        // attacks[((int)main.CurrentAttackType, main.AttackIndex)]?.Invoke();

        mainAnimator.SetInteger("AttackType", 1);
        mainAnimator.SetInteger("AttackIndex",0);
        attacks[(1, 0)]?.Invoke();
    }

    public override void StateExit() { }

    public override void StateUpdate() { }

    public override void StateFixedUpdate() { }

    /* ====================== SIMPLE ATTACKS ====================== */
    private void SimpleUppercut()
    {
        Debug.Log("Current attack = simple uppercut");
        mainAnimator.applyRootMotion = true;
        mainAnimator.SetTrigger("Attack");
        main.LookAtPlayer();
    }

    private void SimpleKick()
    {
        Debug.Log("Current attack = simple kick");
        mainAnimator.applyRootMotion = true;
        mainAnimator.SetTrigger("Attack");
        main.LookAtPlayer();
    }

    private void SimpleStomp()
    {
        Debug.Log("Current attack = simple stomp");
        mainAnimator.applyRootMotion = true;
        mainAnimator.SetTrigger("Attack");
        main.LookAtPlayer();
    }

    /* ======================= SPECIAL ATTACKS ======================= */

    private IEnumerator SpecialSmash()
    {


        Debug.Log("Current attack = Special Smash");
        mainAnimator.SetTrigger("Attack");
        mainAnimator.SetBool(main.ParamAttackSpecialPlay, false);
        mainAnimator.applyRootMotion = false;
        yield return new WaitForSeconds(waitTime);


        // float h = main.GetDistanceToPlayer()/2;
        // float posY;
        Vector3 playerPos = main.Player.position;

        mainNavAgent.enabled = false;
        
        main.LookAtPlayer();
        mainAnimator.SetTrigger(main.ParamAttckSpecialStart);

        while((playerPos - main.transform.position).sqrMagnitude > 2)
        {
            // posY = -10/Mathf.Pow(h, 2) * (main.GetDistanceToPlayer() - h) + 10;
            main.transform.position += main.transform.forward * Time.deltaTime;
            // main.transform.position = new Vector3(main.transform.position.x, posY, main.transform.position.z);
            yield return null;
        }
        mainAnimator.SetBool(main.ParamAttackSpecialPlay, true);
        mainNavAgent.enabled = true;
    }

    private IEnumerator SpecialTatsumaki()
    {
        Debug.Log("Current attack = Special Tatsumaki");
        mainAnimator.SetInteger("AttackIndex", (int)MasaraiMain.AttackSpecial.Tatsumaki);
        mainAnimator.SetTrigger("Attack");
        mainAnimator.applyRootMotion = true;
        yield return new WaitForSeconds(waitTime);
        mainAnimator.SetTrigger(main.ParamAttckSpecialStart);
        main.LookAtPlayer();
    }
}