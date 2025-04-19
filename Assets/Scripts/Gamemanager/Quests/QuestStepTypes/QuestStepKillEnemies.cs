using UnityEngine;

public class QuestStepKillEnemies : QuestStep
{
    private void OnEnable()
    {
        GameEvents.OnAllEnemiesKilled += AllEnemiesKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnAllEnemiesKilled -= AllEnemiesKilled;
    }

    private void AllEnemiesKilled()
    {
        FinishQuestStep();
    }
}