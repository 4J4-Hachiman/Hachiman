/*
    Classe de gestion des etats de Masarai
        - Etat inital
        - Changement d'etat
        - Appliquer la logique de l'etat actuel
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 24/04/2025; 
*/

using UnityEngine;

public class MasaraiStateMachine
{
    public MasaraiBaseState Current { get; private set; }

    public void Initalize(MasaraiBaseState startState, bool init=false)
    {   
        Current = startState;
        Current.StateStart(init);
    }

    public void SwitchState(MasaraiBaseState newState, bool init=false)
    {
        Current.StateExit();
        Current = newState;
        Current.StateStart(init);
    }
    
    public void Update()
    {
        Current.StateUpdate();
    }

    public void FixedUpdate()
    {
        Current.StateFixedUpdate();
    }
}
