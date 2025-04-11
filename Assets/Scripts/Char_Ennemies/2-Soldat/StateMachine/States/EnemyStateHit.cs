/*
    Class hit des ennemis
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 11/04/2025;
*/

using UnityEngine;

public class EnemyStateHit : StateBase
{
    public EnemyStateHit(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }

    public override void StateExit() { }

    public override void StateFixedUpdate() { }

    public override void StateStart(bool init = false) { }

    public override void StateUpdate() { }
}
