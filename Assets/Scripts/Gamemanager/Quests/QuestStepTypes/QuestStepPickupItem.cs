/*
    Class pour des listener pour les step de type item pickup
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

using UnityEngine;

public class QuestStepPickupItem : QuestStep
{
    private void OnEnable()
    {
        GameEvents.OnQuestItemPickedUp += OnQuestItemPickedUp;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestItemPickedUp -= OnQuestItemPickedUp;
    }

    private void OnQuestItemPickedUp()
    {
        Debug.Log("Item has been picked up");
        FinishQuestStep();
    }
}
