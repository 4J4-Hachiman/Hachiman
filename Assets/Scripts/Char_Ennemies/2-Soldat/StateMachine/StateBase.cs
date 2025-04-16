/*
    Class de base pour les etats des StateMachines
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 29/03/2025;
*/

public abstract class StateBase
{
    protected EnemyStateMachine enemyStateMachine;
    protected EnnemiMain ennemiMain;

    public StateBase(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.ennemiMain = ennemiMain;
    }

    public abstract void StateStart(bool init=false);
    public abstract void StateExit();
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate();
}
