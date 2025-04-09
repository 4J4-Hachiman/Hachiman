/*
    Class de base pour les etats des StateMachines
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 29/03/2025;
*/

using UnityEngine;

public abstract class StateBase
{
    protected EnemyStateMachine enemyStateMachine;
    protected EnnemiMain ennemiMain;

    /// <summary>Class state constructor.</summary>
    /// <param name="enemyStateMachine">EnemyStateMachine class reference</param>
    /// <param name="ennemiMain">EnnemiMain class reference</param>
    public StateBase(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.ennemiMain = ennemiMain;
    }
    
    /// <summary>OnOnable method of state.</summary>
    /// <param name="init">Reset the local state variables to their intial value.</param>
    public abstract void StateStart(bool init=false);

    /// <summary>OnDisable method of state.</summary>
    public abstract void StateExit();

    ///<summary> Update method of state. </summary>
    public abstract void StateUpdate();

    /// <summary> FixedUpdate method of state. </summary>
    public abstract void StateFixedUpdate();
}
