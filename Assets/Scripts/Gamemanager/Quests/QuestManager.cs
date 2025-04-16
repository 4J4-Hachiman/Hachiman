using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [field: SerializeField] private QuestData[] dataList;
    private Dictionary<string, Quest> gameQuests;
    private Quest currentQuest;
    
    [Header("Quest Game Objects")]
    [field: SerializeField] private GameObject[] questGameObjects;

    
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
            gameQuests.Add(dataList[i].ID, new Quest(dataList[i]));
        }
    }

    private void QuestStart()
    {
        currentQuest.QuestStart();
    }

    private void LoadNextQuest()
    {
        currentQuest = GetQuestByID(currentQuest.Data.NextQuest.ID);
        QuestStart();
    }

    private void QuestEnd()
    {
        LoadNextQuest();
    }
    
    private Quest GetQuestByID(string id)
    {
        return gameQuests[id];
    }
}
