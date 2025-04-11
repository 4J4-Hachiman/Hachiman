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

        ennemiMain.Gamemanager.Combat.RemoveFromReadyList(ennemiMain);

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
        ennemiMain.LookAtPlayer(200);
        ennemiMain.Agent.destination = ennemiMain.Animator.rootPosition;
    }
    
    private IEnumerator AttackPlayer()
    {
        string aType = ennemiMain.GetRandomAttackType();
        ennemiMain.Animator.SetTrigger(aType);
        yield return new WaitForSeconds(aType == "Attack" ? 1f : 3.75f);
        OnAttackEnd?.Invoke();
        yield break;
    }
}
