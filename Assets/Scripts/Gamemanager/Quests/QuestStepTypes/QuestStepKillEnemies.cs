using UnityEngine;

public class QuestStepKillEnemies : QuestStep
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
        Debug.Log("All enemies have been killed, quest step finished");
        FinishQuestStep();
    }
}