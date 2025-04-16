using System;
using UnityEngine;

public class Quest
{
    public QuestData Data { get; private set; }
    int currentStepIndex;
    private GameObject[] questStepGO;
    public event Action OnQuestOver;

    public Quest(QuestData data)
    {
        Data = data;

        questStepGO = new GameObject[Data.QuestStepGO.Length];
        for (int i = 0; i < Data.QuestStepGO.Length; i++)
        {
            questStepGO[i] = Data.QuestStepGO[i];
        }
    }

    public void QuestStart()
    {
        currentStepIndex = 0;
        StepStart(null);
        GameEvents.OnQuestStepFinished += GetNextStep;
    }

    private void GetNextStep()
    {
        if (currentStepIndex == Data.QuestStepGO.Length - 1)
        {
            QuestOver();
            return;
        }
        
        currentStepIndex++;
        Debug.Log("Going to next step");
        StepStart(null);
    }

    /// <summary> Starts the next quest step byt instantating the appropraie GameObject </summary>
    /// <param name="parent"></param>
    private void StepStart(Transform parent)
    {
        GameObject questGO = GetStepGO();
        UnityEngine.Object.Instantiate(questGO, parent);
    }

    private GameObject GetStepGO()
    {
        return Data.QuestStepGO[currentStepIndex];
    }

    private void QuestOver()
    {
        Debug.Log("<color=green>The current quest is over");
        OnQuestOver?.Invoke();
    }
}