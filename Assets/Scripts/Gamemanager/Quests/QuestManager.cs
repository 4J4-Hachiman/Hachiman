/*
    Class de gestion des quetes du jeu
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [field: SerializeField] private Transform questGameObjectParent;

    [Header("Quests Data")]
    [field: SerializeField] private QuestData[] dataList;

    [field: SerializeField] private TextMeshProUGUI uiQuestNameDisplay;
    [field: SerializeField] private TextMeshProUGUI uiQuestStepDisplay;

    private Dictionary<string, Quest> gameQuests;
    private static Quest currentQuest;
    public static string CurrentQuestID { get { return currentQuest.Data.ID; } }
    public static int CurrentQuestStepIndex { get { return currentQuest.GetStepIndex(); } }

    private void Awake()
    {
        gameQuests = new();
        LoadQuests();
        currentQuest = GetQuestByID(dataList[0].ID);
        QuestStart();
    }

    private void LoadQuests()
    {
        for (int i = 0; i < dataList.Length; i++)
        {
            gameQuests.Add(dataList[i].ID, new Quest(this, dataList[i], questGameObjectParent));
        }
    }
        private void QuestStart()
    {
        Debug.Log($"New quest ID  = {CurrentQuestID}");
        currentQuest.QuestStart();
        currentQuest.OnQuestOver += QuestEnd;
    }

    private void QuestEnd()
    {
        currentQuest.OnQuestOver -= QuestEnd;
        LoadNextQuest();
    }

    private void LoadNextQuest()
    {
        if (!currentQuest.Data.NextQuest)
        {
            Debug.Log("NO MORE QUESTS ARE AVAILABLE");
            uiQuestNameDisplay.text = "ALL_QUESTS_ARE_ACCOMPLISHED";
            uiQuestStepDisplay.text = "NO_MORE_QUESTS";
            return;
        }

        currentQuest = GetQuestByID(currentQuest.Data.NextQuest.ID);
        QuestStart();
    }

    private Quest GetQuestByID(string id)
    {
        return gameQuests[id];
    }

    public void UpdateQuestUI()
    {
        uiQuestNameDisplay.text = currentQuest.Data.ID;
        uiQuestStepDisplay.text = currentQuest.Data.QuestStepInfo[CurrentQuestStepIndex];
    }
}