using UnityEngine;

public class QuestStepReachLocation : QuestStep
{
    private void OnEnable()
    {
        Debug.Log("Quest step started : " + name);
        GameEvents.OnAllEnemiesKilled += AllEnemiesKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnAllEnemiesKilled -= AllEnemiesKilled;
    }

    private void AllEnemiesKilled()
    {
        Debug.Log("ReachedLocation ish - quest step finished");
    }
}
