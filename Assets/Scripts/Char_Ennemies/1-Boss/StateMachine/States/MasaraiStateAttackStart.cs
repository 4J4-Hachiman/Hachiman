using UnityEngine;

public class MasaraiStateAttackStart : MasaraiBaseState
{
    private int waitTime;

    public MasaraiStateAttackStart(MasaraiStateMachine masaraiSM, MasaraiMain main) : base(masaraiSM, main) { }

    public override void StateStart(bool init = false)
    {
        waitTime = 3;
    }

    public override void StateExit()
    {

    }

    public override void StateUpdate()
    {

    }
    public override void StateFixedUpdate()
    {
        
    }

    
}
