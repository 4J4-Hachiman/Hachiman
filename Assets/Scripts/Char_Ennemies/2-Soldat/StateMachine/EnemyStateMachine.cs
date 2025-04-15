/*
    Classe de gestion des etats des ennemis
        - Etat inital
        - Changement d'etat
        - Appliquer la logique de l'etat actuel
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 27/03/2025; 
*/

using UnityEngine;


public class EnemyStateMachine
{    
    public StateBase currentState { get; private set; }
    public void Initalize(StateBase startState, bool init=false)
    {   
        // Reference to the starting state
        currentState = startState;
        currentState.StateStart(init);
    }
    public void SwitchState(StateBase newState, bool init=false)
    {
        currentState.StateExit();
        currentState = newState;
        currentState.StateStart(init);
    }
    
    public void Update()
    {
        currentState.StateUpdate();
    }
    public void FixedUpdate()
    {
        currentState.StateFixedUpdate();
    }
}
