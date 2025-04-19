using UnityEngine;

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
