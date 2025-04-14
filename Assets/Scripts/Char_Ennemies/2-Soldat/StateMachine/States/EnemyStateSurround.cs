/*
    Logique des ennemis lorsqu'ils encerclent le joueur
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 12/04/2025;
*/

using UnityEngine;

public class EnemyStateSurround : StateBase
{
    private Vector3 lockOffSet;

    /* ========================= */
    public EnemyStateSurround(EnemyStateMachine enemyStateMachine, EnnemiMain ennemiMain) : base(enemyStateMachine, ennemiMain) { }

    public override void StateStart(bool init)
    {
        if (init)
        {
            lockOffSet = ennemiMain.Gamemanager.Combat.GetNewSurroundPos();
        }

        ennemiMain.Gamemanager.Combat.AddToReadyList(ennemiMain);
        ennemiMain.Agent.updateRotation = true;
    }

    public override void StateExit() { }

    public override void StateUpdate() { }

    public override void StateFixedUpdate()
    {
        ennemiMain.LookAtPlayer();
        SurroundPlayer();
    }

    /* ============================== PRIVATE VARIABLES ============================== */

    private void SurroundPlayer()
    {
        if (!IsWithinAcceptableDistance())
        {
            ennemiMain.SetNavDestination(ennemiMain.Player.transform.position + lockOffSet);
        }
    }

    private bool IsWithinAcceptableDistance()
    {
        // if enemy is too close
        if (GetMagnitude(ennemiMain.transform.position, ennemiMain.Player.transform.position) < 1)
        {
            return false;
        }

        if (GetMagnitude(ennemiMain.transform.position, ennemiMain.Player.transform.position + lockOffSet) > ennemiMain.LockOffsetThreshold)
        {
            if (GetMagnitude(ennemiMain.transform.position, ennemiMain.Player.transform.position) > 25)
            {
                return false;
            }
        }
        return true;
    }

    private float GetMagnitude(Vector3 v1, Vector3 v2)
    {
        return (v2 - v1).sqrMagnitude;
    }
}
