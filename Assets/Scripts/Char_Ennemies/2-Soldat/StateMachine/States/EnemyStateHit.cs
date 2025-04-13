/*
    Class hit des ennemis
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 12/04/2025;
*/

using UnityEngine;
using System;

public class EnemyStateHit : StateBase
{
    public event Action OnHitAnimationEnd;

    public EnemyStateHit(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }

    public override void StateStart(bool init = false)
    {
        // Debug.Log("<color=orange>Currently in hit state</color>");
        ennemiMain.OnAnimationEnd += HandleOnAnimationEnd;
        ennemiMain.Animator.applyRootMotion = true;
        ennemiMain.Agent.updatePosition = false;
        ennemiMain.Agent.updateRotation = false;
        ennemiMain.Animator.SetFloat("HitAnim", UnityEngine.Random.Range(0, 4));
        ennemiMain.Animator.SetTrigger("Hit");
    }

    public override void StateExit()
    {
        ennemiMain.Animator.applyRootMotion = false;
        ennemiMain.Agent.updatePosition = true;
        ennemiMain.Agent.updateRotation = true;
        ennemiMain.OnAnimationEnd -= HandleOnAnimationEnd;
    }
    public override void StateUpdate() { }

    public override void StateFixedUpdate() { }
    
    private void HandleOnAnimationEnd()
    {
        OnHitAnimationEnd?.Invoke();
    }
}