using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [field: SerializeField] private Transform questGameObjectParent;

    [Header("Quests Data")]
    [field: SerializeField] private QuestData[] dataList;
    // [field: SerializeField] private GameObject[] questGameObjects;

    private Dictionary<string, Quest> gameQuests;
    private static Quest currentQuest;
    public static string CurrentQuestID { get { return currentQuest.Data.ID; } }
    public static int CurrentQuestStepIndex { get { return currentQuest.GetStepIndex(); }}

    private void Awake()
    {
        gameQuests = new();
        LoadQuests();
        currentQuest = GetQuestByID(dataList[0].ID);
        QuestStart();
    }

    /// <summary>Methode qui charge toutes les quetes du jeu. </summary>
    private void LoadQuests()
    {
        for (int i = 0; i < dataList.Length; i++)
        {
            gameQuests.Add(dataList[i].ID, new Quest(dataList[i], questGameObjectParent));
        }
    }

    /// <summary>Appel la method QuestStart() de la quete actuelle. </summary>
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
            return;
        }
        
        currentQuest = GetQuestByID(currentQuest.Data.NextQuest.ID);
        QuestStart();
    }

    private Quest GetQuestByID(string id)
    {
        return gameQuests[id];
    }
}