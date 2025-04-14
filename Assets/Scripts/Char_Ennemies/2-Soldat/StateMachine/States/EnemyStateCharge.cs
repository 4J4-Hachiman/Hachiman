/*
    Class de gestion de la logique des ennemis lorsqu'ils chargent/attaquent 
    le joueur.
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 01/04/2025;
*/

using System.Collections;
using UnityEngine;

public class EnemyStateCharge : StateBase
{
    public EnemyStateCharge(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }
    
    public override void StateStart(bool init)
    {
        // Debug.Log("<color=orange>Charging PLAYER");
        ennemiMain.Animator.SetTrigger("Charge");
        ennemiMain.SetNavVitesse(5);
        ennemiMain.StartCoroutine(EngagePlayer());
    }

    public override void StateExit() { }

    public override void StateUpdate()
    {
        ennemiMain.LookAtPlayer();
    }

    public override void StateFixedUpdate() { }

    private IEnumerator EngagePlayer()
    {
        while (!ennemiMain.IsWithinAttackDistance())
        {
            ennemiMain.SetNavDestination(ennemiMain.Player.transform.position);
            yield return null;
        }
        ennemiMain.StateMachine.SwitchState(ennemiMain.StateAttack);
    }
}
