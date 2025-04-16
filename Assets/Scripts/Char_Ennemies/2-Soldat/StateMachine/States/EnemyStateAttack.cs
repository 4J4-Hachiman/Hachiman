/*
    Logique des ennemis lorsqu'ils sont en etat d'attaque
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 12/04/2025;
*/

using System;

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
        attackType = ennemiMain.GetRandomAttackType();
        
        if (attackType == EnnemiMain.AttackTypes.AttackSimple)
        {
            int i = UnityEngine.Random.Range(0, 4);
            ennemiMain.Animator.SetInteger("AttackIndex", i);
            ennemiMain.Animator.SetTrigger("Attack");
            ennemiMain.OnAnimationEnd += HandleOnAnimationEnd;
        }
        else
        {
            ennemiMain.OnComboStepStart += HandleOnComboStepStart;
            ennemiMain.Animator.SetTrigger("AttackCombo");
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
        ennemiMain.Animator.SetBool("ContinueCombo", performNext);
        ennemiMain.LookAtPlayer(100);
        
        if (!performNext)
        {
            ennemiMain.OnAnimationEnd += HandleOnAnimationEnd;
        }
    }
    
    private void HandleOnAnimationEnd()
    {
        ennemiMain.OnAnimationEnd -= HandleOnAnimationEnd;
        OnAttackEnd?.Invoke();
    }
}