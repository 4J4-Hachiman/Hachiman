using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [field: SerializeField] private QuestData[] dataList;
    private Dictionary<string, Quest> gameQuests;
    private static Quest currentQuest;
    
    [Header("Quest Game Objects")]
    [field: SerializeField] private GameObject[] questGameObjects;


    private void Awake()
    {
        gameQuests = new();
        LoadQuests();

        currentQuest = GetQuestByID(dataList[0].ID);
        QuestStart();
    }

    public static string GetCurrentQuestID()
    {
        return currentQuest.Data.ID;
    }

    /// <summary>Methode qui charge toutes les quetes du jeu. </summary>
    private void LoadQuests()
    {
        for (int i = 0; i < dataList.Length; i++)
        {
            gameQuests.Add(dataList[i].ID, new Quest(dataList[i]));
        }
    }

    /// <summary>Appel la method QuestStart() de la quete actuelle. </summary>
    private void QuestStart()
    {
        Debug.Log($"New quest ID  = {GetCurrentQuestID()}");
        currentQuest.QuestStart();
        currentQuest.OnQuestOver += QuestEnd;
    }

    private void QuestEnd()
    {
        currentQuest.OnQuestOver -= QuestEnd;
        Debug.Log("Current quest is over !");
        LoadNextQuest();
    }

    private void LoadNextQuest()
    {
        currentQuest = GetQuestByID(currentQuest.Data.NextQuest.ID);
        QuestStart();
    }

    private Quest GetQuestByID(string id)
    {
        return gameQuests[id];
    }
}