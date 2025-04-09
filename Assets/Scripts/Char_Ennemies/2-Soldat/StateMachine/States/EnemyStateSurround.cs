/*
    Logique des ennemis lorsqu'ils encerclent le joueur
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 29/03/2025;
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
            lockOffSet = GetNewOffset();
        }
        
        Debug.Log("Entered surround state");
        ennemiMain.Gamemanager.Combat.AddToReadyList(ennemiMain);
        ennemiMain.Agent.updateRotation = false;
    }

    public override void StateExit() { }

    public override void StateUpdate()
    {
        ennemiMain.LookAtPlayer();
        SurroundPlayer();
    }

    public override void StateFixedUpdate() { }

    /* ============================== PRIVATE VARIABLES ============================== */

    private void SurroundPlayer()
    {
        if (!IsWithinAcceptableDistance())
        {
            ennemiMain.SetNavDestination(ennemiMain.Player.transform.position + lockOffSet);
        }
    }

    private Vector3 GetNewOffset()
    {
        return ennemiMain.Gamemanager.Combat.GetNewSurroundPos();
    }

    private float GetMagnitude(Vector3 v1, Vector3 v2)
    {
        return (v2 - v1).sqrMagnitude;
    }

    private bool IsWithinAcceptableDistance()
    {
        if (GetMagnitude(ennemiMain.transform.position, ennemiMain.Player.transform.position) < 3)
        {
            return false;
        }

        if (GetMagnitude(ennemiMain.transform.position, ennemiMain.Player.transform.position + lockOffSet) > ennemiMain.LockOffsetThreshold)
        {
            if (GetMagnitude(ennemiMain.transform.position, ennemiMain.Player.transform.position) > 12)
            {
                return false;
            }
        }

        return true;
    }
}
