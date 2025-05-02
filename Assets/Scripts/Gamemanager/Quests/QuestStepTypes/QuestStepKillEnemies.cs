/*
    Class pour des listener pour les step de type kill all ennemis
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

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