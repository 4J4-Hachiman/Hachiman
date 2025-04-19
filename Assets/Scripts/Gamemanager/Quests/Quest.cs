using System;
using UnityEngine;

public class Quest
{
    public QuestData Data { get; }
    private int currentStepIndex;
    private GameObject[] questStepGO;
    public event Action OnQuestOver;
    private readonly Transform parentGO;

    public Quest(QuestData data, Transform parentGO)
    {
        Data = data;
        this.parentGO = parentGO;
        questStepGO = new GameObject[Data.QuestStepGO.Length];
        
        for (int i = 0; i < Data.QuestStepGO.Length; i++)
        {
            questStepGO[i] = Data.QuestStepGO[i];
        }
    }

    public void QuestStart()
    {
        currentStepIndex = 0;
        StepStart(parentGO);
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
        StepStart(parentGO);
    }

    private void StepStart(Transform parent)
    {
        GameObject questGO = GetStepGO();
        UnityEngine.Object.Instantiate(questGO, parent);
        Debug.Log($"NEW QUEST OBJECTIVE : {Data.QuestStepInfo[currentStepIndex]}");
    }

    private GameObject GetStepGO()
    {
        return Data.QuestStepGO[currentStepIndex];
    }

    private void QuestOver()
    {
        // Debug.Log($"<color=green> Quest {QuestManager.CurrentQuestID} over </color>");
        GameEvents.OnQuestStepFinished -= GetNextStep;
        OnQuestOver?.Invoke();
    }

    public int GetStepIndex()
    {
        return currentStepIndex;
    }
}