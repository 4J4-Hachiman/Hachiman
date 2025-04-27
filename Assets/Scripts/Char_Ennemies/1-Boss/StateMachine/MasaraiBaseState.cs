/*
    Class de base pour les etats des StateMachines de Masarai
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 24/04/2025;
*/

public abstract class MasaraiBaseState
{
    protected MasaraiStateMachine masaraiSM;
    protected MasaraiMain main;

    public MasaraiBaseState(MasaraiStateMachine masaraiSM, MasaraiMain main)
    {
        this.masaraiSM = masaraiSM;
        this.main = main;
    }

    public abstract void StateStart(bool init=false);
    public abstract void StateExit();
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate();
}
