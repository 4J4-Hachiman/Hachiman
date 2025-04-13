/*
    Logique des ennemis lorsqu'ils sont en etat d'attaque
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 12/04/2025;
*/

using System;
using UnityEngine;

public class EnemyStateAttack : StateBase
{
    public event Action OnAttackEnd;
    EnnemiMain.AttackTypes attackType;

    public EnemyStateAttack(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }

    public override void StateStart(bool init)
    {

        ennemiMain.Animator.applyRootMotion = true;
        ennemiMain.Agent.updatePosition = false;
        ennemiMain.Agent.speed = 0;
        ennemiMain.Agent.destination = ennemiMain.transform.position;
        ennemiMain.Gamemanager.Combat.RemoveFromReadyList(ennemiMain);

        attackType = ennemiMain.GetRandomAttackType();

        if (attackType == EnnemiMain.AttackTypes.AttackSimple)
        {
            int i = UnityEngine.Random.Range(0, 4);
            ennemiMain.Animator.SetInteger("AttackIndex", i);
            ennemiMain.Animator.SetTrigger("Attack");
            ennemiMain.OnAnimationEnd += HandleOnAnimationEnd;

            // Debug.Log("Will perform a <color=green>SIMPLE</color> attack");
        }
        else
        {
            ennemiMain.OnComboStepStart += HandleOnComboStepStart;
            ennemiMain.Animator.SetTrigger("AttackCombo");
            // ennemiMain.Animator.SetBool("ContinueCombo", true);
            // Debug.Log("Will perform a <color=green>COMBO</color> attack");
        }
    }

    public override void StateExit()
    {
        ennemiMain.OnComboStepStart -= HandleOnComboStepStart;
        ennemiMain.OnAnimationEnd -= HandleOnAnimationEnd;
        ennemiMain.Animator.applyRootMotion = false;
        ennemiMain.Agent.updatePosition = true;
    }

    public override void StateUpdate() 
    { 
        if((ennemiMain.Player.transform.position - ennemiMain.transform.position).sqrMagnitude > 1)
        {
            ennemiMain.Agent.nextPosition = ennemiMain.transform.position;
        }
    }

    public override void StateFixedUpdate() { }

    private void HandleOnComboStepStart(bool performNext)
    {
        // ennemiMain.Animator.applyRootMotion = (ennemiMain.Player.transform.position - ennemiMain.transform.position).sqrMagnitude > 1;
        ennemiMain.Animator.SetBool("ContinueCombo", performNext);
        ennemiMain.LookAtPlayer(100);
        
        if (!performNext)
        {
            // Debug.Log("<color=yellow>End of combo");
            ennemiMain.OnAnimationEnd += HandleOnAnimationEnd;
        }
    }

    private void HandleOnAnimationEnd()
    {
        // Debug.Log("<color=green>Attack is over</color>");
        ennemiMain.OnAnimationEnd -= HandleOnAnimationEnd;
        OnAttackEnd?.Invoke();
    }
}