/*
    Class d'état de mort
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 15/04/2025;
*/

public class EnemyStateDead : StateBase
{
    public EnemyStateDead(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }

    public override void StateStart(bool init = false)
    {
        // ennemiMain.Animator.SetTrigger("Dead");
        ennemiMain.SetNavVitesse(0);
        ennemiMain.Agent.updateRotation = false;
        ennemiMain.CapsuleCollider.enabled = false;
    }

    public override void StateExit() { }
    public override void StateUpdate() { }
    public override void StateFixedUpdate() { }
}
