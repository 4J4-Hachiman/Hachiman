using UnityEngine;

public class MasaraiStateWalk : MasaraiBaseState
{
    public MasaraiStateWalk(MasaraiStateMachine masaraiSM, MasaraiMain main) : base(masaraiSM, main) { }

    public override void StateStart(bool init = false) { }

    public override void StateExit() { }

    public override void StateFixedUpdate()
    {
        Debug.Log("Masarai State FixedUpdate performed.");
        main.SetNavAgentDestination(main.Player.position);
    }
    
    public override void StateUpdate() { }
}