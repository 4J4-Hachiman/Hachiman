using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MasaraiStateAttack : MasaraiBaseState
{
    private readonly Animator mainAnimator;
    private readonly NavMeshAgent mainNavAgent;
    private readonly SphereCollider mainHitBox;
    private readonly Dictionary<(int, int), Action> attacks;
    private bool trackPlayer = true;

    public MasaraiStateAttack(MasaraiStateMachine masaraiSM, MasaraiMain main, Animator mainAnimator, NavMeshAgent mainNavAgent, SphereCollider mainHitBox) : base(masaraiSM, main)
    {
        this.mainAnimator = mainAnimator;
        this.mainNavAgent = mainNavAgent;
        this.mainHitBox = mainHitBox;

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
        mainAnimator.SetBool(main.ParamAttackSpecialPlay, false);
        mainNavAgent.updatePosition = false;
        mainAnimator.SetInteger("AttackType", (int)main.CurrentAttackType);
        mainAnimator.SetInteger("AttackIndex", main.AttackIndex);
        attacks[((int)main.CurrentAttackType, main.AttackIndex)]?.Invoke();
    }

    public override void StateExit() { }

    public override void StateUpdate() { }

    public override void StateFixedUpdate() 
    { 
        if (trackPlayer)
        {
            main.LookAtPlayer();
        }
    }

    /* ====================== SIMPLE ATTACKS ====================== */
    private void SimpleUppercut()
    {   
        mainHitBox.radius = 0.35f;
        mainAnimator.applyRootMotion = true;
        mainAnimator.SetTrigger("Attack");
        trackPlayer = false; 
        main.LookAtPlayer();
    }

    private void SimpleKick()
    {
        mainHitBox.radius = 0.35f;
        mainAnimator.applyRootMotion = true;
        mainAnimator.SetTrigger("Attack");
        trackPlayer = false; 
        main.LookAtPlayer();
    }

    private void SimpleStomp()
    {
        mainHitBox.radius = 1.25f;
        mainAnimator.applyRootMotion = true;
        mainAnimator.SetTrigger("Attack");
        trackPlayer = false; 
        main.LookAtPlayer();
    }

    /* ======================= SPECIAL ATTACKS ======================= */

    private IEnumerator SpecialSmash()
    {
        mainHitBox.radius = 2.75f;
        mainAnimator.SetTrigger("Attack");
        mainAnimator.applyRootMotion = false;
        mainNavAgent.enabled = false;
        trackPlayer = false;
        yield return new WaitForSeconds(3);
        mainAnimator.SetBool("AttackSpecialPlay", true);
        Vector3 p1 = main.transform.position;
        p1.y = 0;

        Vector3 p2 = main.Player.position;
        float time = 0.75f;
        float delta = Vector3.Distance(p2, main.transform.position) / time;
        float h = Vector3.Distance(p2, p1) / 2;
        float k = 5;
        float remain = 2 * h;
        main.LookAtPlayer();

        while (time > 0)
        {
            main.transform.position += main.transform.forward * ( delta * Time.deltaTime);
            p1 = main.transform.position;
            p1.y = 0;
            remain -= delta * Time.deltaTime;
            float x = Vector3.Distance(p2, p1);
            if (time < 0.4f)
            {
                mainAnimator.SetBool("AttackSpecialPlay", false);
            }
            float pY = -k / Mathf.Pow(h, 2) * Mathf.Pow(x - h, 2) + k;
            main.transform.position = new Vector3(main.transform.position.x, pY, main.transform.position.z);
            time -= 1 * Time.deltaTime;
            yield return null;
        }
        
        mainAnimator.SetBool(main.ParamAttackSpecialPlay, false);
        mainNavAgent.enabled = true;
        main.transform.position = mainNavAgent.nextPosition;
    }
    
    private IEnumerator SpecialTatsumaki()
    {
        mainHitBox.radius = 1.5f;
        mainAnimator.SetInteger("AttackIndex", (int)MasaraiMain.AttackSpecial.Tatsumaki);
        mainAnimator.SetTrigger("Attack");
        mainAnimator.applyRootMotion = true;
        trackPlayer = true;
        yield return new WaitForSeconds(1);
        mainAnimator.SetTrigger(main.ParamAttckSpecialStart);
        main.LookAtPlayer();
        
        while ((main.Player.position - main.transform.position).sqrMagnitude > 5f)
        {
            yield return null;
        }

        trackPlayer = false;
    }
}