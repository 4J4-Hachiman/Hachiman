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
    public event Action OnAttackEnd;
    public EnemyStateAttack(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }

    public override void StateStart(bool init)
    {
        ennemiMain.SetNavVitesse(0);
        ennemiMain.Animator.applyRootMotion = true;
        ennemiMain.StartCoroutine(AttackPlayer());
        ennemiMain.Gamemanager.Combat.AddToReadyList(ennemiMain);
    }

    public override void StateExit()
    {
        ennemiMain.SetNavVitesse(ennemiMain.vitesseDeplacement);
        ennemiMain.Animator.applyRootMotion = false;
    }
    
    public override void StateUpdate() { }

    public override void StateFixedUpdate() { }

    private IEnumerator AttackPlayer()
    {
        ennemiMain.Animator.SetTrigger("Attack");
        yield return new WaitForSeconds(2.5f);
        OnAttackEnd?.Invoke();
        yield break;
    }
}
