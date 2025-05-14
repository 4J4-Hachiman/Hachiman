/*
    Class generale pour toute les quetes du jeu
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

using System;
using UnityEngine;

public class Quest
{
    private readonly QuestManager questManager;
    public QuestData Data { get; }
    private int currentStepIndex;
    private GameObject[] questStepGO;
    public event Action OnQuestOver;
    private readonly Transform parentGO;

    public Quest(QuestManager questManager, QuestData data, Transform parentGO)
    {
        this.questManager = questManager;
        Data = data;
        this.parentGO = parentGO;
        questStepGO = new GameObject[Data.QuestStepGO.Length];
        Data.state = QuestStates.Inactive;
        
        for (int i = 0; i < Data.QuestStepGO.Length; i++)
        {
            questStepGO[i] = Data.QuestStepGO[i];
        }
    }

    public void QuestStart()
    {
        currentStepIndex = 0;
        StepStart(parentGO);
        Data.state = QuestStates.Active;
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
        questManager.UpdateQuestUI();
    }

    private GameObject GetStepGO()
    {
        return Data.QuestStepGO[currentStepIndex];
    }

    private void QuestOver()
    {
        GameEvents.OnQuestStepFinished -= GetNextStep;
        Data.state = QuestStates.Completed;
        OnQuestOver?.Invoke();
    }

    public int GetStepIndex()
    {
        return currentStepIndex;
    }
}