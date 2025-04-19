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
        questManager.UpdateQuestUI();
        Debug.Log($"NEW QUEST OBJECTIVE : {Data.QuestStepInfo[currentStepIndex]}");
    }

    private GameObject GetStepGO()
    {
        return Data.QuestStepGO[currentStepIndex];
    }

    private void QuestOver()
    {
        GameEvents.OnQuestStepFinished -= GetNextStep;
        OnQuestOver?.Invoke();
    }

    public int GetStepIndex()
    {
        return currentStepIndex;
    }
}