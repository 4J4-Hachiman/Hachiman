using UnityEngine;

public class QuestStepReachLocation : QuestStep
{
    private void OnEnable()
    {
        Debug.Log("Quest step started : " + name);
        GameEvents.OnLocationReached += OnLocationReached;
    }

    private void OnDisable()
    {
        GameEvents.OnLocationReached -= OnLocationReached;
    }
    
    private void OnLocationReached()
    {
        Debug.Log("ReachedLocation ish - quest step finished");
        FinishQuestStep();
    }
}
