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
