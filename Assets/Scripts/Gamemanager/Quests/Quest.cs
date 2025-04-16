using Unity.VisualScripting;
using UnityEngine;

public class Quest
{
    public QuestData Data { get; private set; }
    int currentStepIndex;
    QuestStep[] questSteps;

    public Quest(QuestData data)
    {
        Data = data;
        currentStepIndex = 0;
    }

    public void QuestStart()
    {
        IterateQuestGO(null);
        Debug.Log("Starting Quest");
        GameEvents.OnQuestStepFinished += IncrementStep;
    }

    public void IncrementStep()
    {
        if (currentStepIndex == Data.QuestStepGO.Length - 1)
        {
            QuestOver();
            return;
        }

        Debug.Log("Going to next step");
        currentStepIndex++;
        IterateQuestGO(null);
    }

    public GameObject GetStepGO()
    {
        return Data.QuestStepGO[currentStepIndex];
    }

    public void IterateQuestGO(Transform parent)
    {
        GameObject questGO = GetStepGO();
        Object.Instantiate(questGO, parent);
    }

    public void QuestOver()
    {
        Debug.Log("The current quest is over");
    }
}