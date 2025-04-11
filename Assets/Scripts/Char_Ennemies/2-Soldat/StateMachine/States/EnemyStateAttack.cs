/*
    Logique des ennemis lorsqu'ils sont en etat d'attaque
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 29/03/2025;
*/

using System;
using System.Collections;
using UnityEngine;

public class EnemyStateAttack : StateBase
{
    int i;
    public event Action OnAttackEnd;
    public EnemyStateAttack(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }

    public override void StateStart(bool init)
    {
        i = 0;
        ennemiMain.Animator.applyRootMotion = true;
        ennemiMain.Agent.updatePosition = false;
        ennemiMain.OnComboStepEnd += HandleComboStepEnd;
        ennemiMain.Gamemanager.Combat.AddToReadyList(ennemiMain);
        ennemiMain.StartCoroutine(AttackPlayer());
    }

    public override void StateExit()
    {
        ennemiMain.SetNavVitesse(ennemiMain.vitesseDeplacement);
        ennemiMain.Animator.applyRootMotion = false;
        ennemiMain.Agent.updatePosition = true;
        ennemiMain.OnComboStepEnd -= HandleComboStepEnd;
    }

    public override void StateUpdate() { }

    public override void StateFixedUpdate() { }

    private void HandleComboStepEnd()
    {
        ennemiMain.LookAtPlayer(100);
        ennemiMain.Agent.destination = ennemiMain.Animator.rootPosition;
        ennemiMain.Agent.updatePosition = true;
        ennemiMain.Agent.updatePosition = false;
    }

    private IEnumerator AttackPlayer()
    {
        ennemiMain.Animator.SetTrigger(ennemiMain.GetRandomAttackType());
        yield return new WaitForSeconds(4f);
        OnAttackEnd?.Invoke();
        yield break;
    }
}
