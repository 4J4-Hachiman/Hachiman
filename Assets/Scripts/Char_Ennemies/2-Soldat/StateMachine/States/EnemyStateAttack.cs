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
        ennemiMain.OnComboStepEnd += HandleComboStepEnd;
        ennemiMain.SetNavVitesse(0);
        ennemiMain.Gamemanager.Combat.AddToReadyList(ennemiMain);
        ennemiMain.StartCoroutine(AttackPlayer());
    }

    public override void StateExit()
    {
        ennemiMain.SetNavVitesse(ennemiMain.vitesseDeplacement);
        ennemiMain.Animator.applyRootMotion = false;
        ennemiMain.OnComboStepEnd -= HandleComboStepEnd;

    }
    
    public override void StateUpdate() { }

    public override void StateFixedUpdate() { }

    private void HandleComboStepEnd()
    {
        i++;
        Debug.Log($"Combo {i} Step Over, {ennemiMain.Animator.applyRootMotion}");
        ennemiMain.LookAtPlayer();
    }

    private IEnumerator AttackPlayer()
    {
        ennemiMain.Animator.SetTrigger("Attack_Combo");
        yield return new WaitForSeconds(5f);
        OnAttackEnd?.Invoke();
        yield break;
    }
}
