/*
    Class de gestion de l'etat patrouille des ennemis
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 01/04/2025;
*/

using System;

public class EnemyStatePatrol : StateBase
{
    private int index;
    public event Action OnPlayerSpotted;

    public EnemyStatePatrol(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }

    public override void StateStart(bool init = false)
    {
        if (init)
        {   
            index = 0;
            GoToNextPatrolPosition();
        }

        ennemiMain.SetNavVitesse(ennemiMain.vitesseDeplacement);
    }

    public override void StateExit() { }
    
    public override void StateUpdate() 
    { 
        if ((ennemiMain.Agent.destination - ennemiMain.transform.position).sqrMagnitude < 0.5f)
        {
            GoToNextPatrolPosition();
        }
    }

    public override void StateFixedUpdate() 
    { 
        if (ennemiMain.GetSpottedPlayer())
        {
            OnPlayerSpotted?.Invoke();
        }
    }

    private void GoToNextPatrolPosition()
    {
        index = (index + 1) % ennemiMain.patrol.Length;
        ennemiMain.SetNavDestination(ennemiMain.patrol[index]);
    }
}
