/*
    Class pour des listener pour les step de type reach location
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

public class QuestStepReachLocation : QuestStep
{
    private void OnEnable()
    {
        GameEvents.OnLocationReached += OnLocationReached;
    }

    private void OnDisable()
    {
        GameEvents.OnLocationReached -= OnLocationReached;
    }

    private void OnLocationReached()
    {
        FinishQuestStep();
    }
}
