/*
    Class pour des listener pour les step de type reach location
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

public class QuestStepWait : QuestStep
{
    private void OnEnable()
    {
        Invoke(nameof(OnWaitTimeOver), 70);
    }

    private void OnWaitTimeOver()
    {
        FinishQuestStep();
    }
}
