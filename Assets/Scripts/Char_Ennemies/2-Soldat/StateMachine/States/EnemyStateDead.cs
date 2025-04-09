using UnityEngine;

public class EnemyStateDead : StateBase
{
    public EnemyStateDead(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }

    public override void StateStart(bool init = false)
    {
        Debug.Log("IS DEAD");
        ennemiMain.SetNavVitesse(0);
        ennemiMain.Animator.SetTrigger("Dead");
        ennemiMain.Agent.updateRotation = false;
        ennemiMain.CapsuleCollider.enabled = false;
    }

    public override void StateExit() { }
    public override void StateUpdate() { }
    public override void StateFixedUpdate() { }
}
