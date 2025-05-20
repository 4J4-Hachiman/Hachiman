/*
    Class de gestion des quetes du jeu
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 14/05/2025;
*/

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour, IDataSaveable
{
    [field: SerializeField] private Transform questGameObjectParent;
    [Header("Quests Data")]
    [field: SerializeField] private QuestData[] questDataList;
    [field: SerializeField] private TextMeshProUGUI uiQuestNameDisplay;
    [field: SerializeField] private TextMeshProUGUI uiQuestStepDisplay;
    private Dictionary<string, Quest> gameQuests;
    private static Quest currentQuest;
    public static string CurrentQuestID { get { return currentQuest.Data.ID; } }
    public static int CurrentQuestStepIndex { get { return currentQuest.GetStepIndex(); } }
    private int _currentQuestStepIndex;
    private GameObject instatiatedGo;

    [field: SerializeField] ChangementCinematiques cutscenePlayer;

    private void Awake()
    {
        gameQuests = new();
        LoadQuests();
        currentQuest = GetQuestByID(questDataList[0].ID);
    }

    private void LoadQuests()
    {
        for (int i = 0; i < questDataList.Length; i++)
        {
            gameQuests.Add(questDataList[i].ID, new Quest(this, questDataList[i], questGameObjectParent));
            gameQuests[questDataList[i].ID].OnQuestOver -= QuestEnd;
        }
    }

    private void QuestStart()
    {
        currentQuest.QuestStart(0);
        if (currentQuest.Data.Inst)
        {
            instatiatedGo = Instantiate(currentQuest.Data.Inst);
            instatiatedGo.transform.SetPositionAndRotation(currentQuest.Data.InstPosition, currentQuest.Data.InstRotation);
            instatiatedGo.SetActive(true);
        }
        currentQuest.OnQuestOver += QuestEnd;
    }

    private void QuestEnd()
    {
        currentQuest.OnQuestOver -= QuestEnd;
        if (currentQuest.Data.CutsceneToPlay)
        {
            cutscenePlayer.DemarrerCinematique(currentQuest.Data.CutsceneToPlay);
            StartCoroutine(WaitForCutsceneEnd());
            return;
        }
        instatiatedGo = null;
        LoadNextQuest();
    }

    private IEnumerator WaitForCutsceneEnd()
    {
        yield return new WaitForSecondsRealtime((float)currentQuest.Data.CutsceneToPlay.length);
        Destroy(instatiatedGo);
        instatiatedGo = null;
        LoadNextQuest();
    }

    private void LoadNextQuest()
    {
        if (!currentQuest.Data.NextQuest)
        {
            uiQuestNameDisplay.text = "ALL_QUESTS_ARE_ACCOMPLISHED";
            uiQuestStepDisplay.text = "NO_MORE_QUESTS";
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Gamemanager>().GestionFin.ArreterJeu();
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
        uiQuestNameDisplay.text = currentQuest.Data.DisplayName;
        uiQuestStepDisplay.text = currentQuest.Data.QuestStepInfo[CurrentQuestStepIndex];
    }

    public void LoadData(GameData data)
    {
        _currentQuestStepIndex = data.currentQuestStep;
        for (int i = 0; i < gameQuests.Count; i++)
        {
            gameQuests.ElementAt(i).Value.Data.state = (QuestStates)data.questStates.GetKey(gameQuests.ElementAt(i).Key, (int)gameQuests.ElementAt(i).Value.Data.state);
        }
        currentQuest = GetQuestByID(data.activeQuest);
        QuestStart();
    }

    public void SaveData(ref GameData data)
    {
        data.activeQuest = CurrentQuestID;
        data.currentQuestStep = CurrentQuestStepIndex;
        for (int i = 0; i < gameQuests.Count; i++)
        {
            data.questStates.SetPair(gameQuests.ElementAt(i).Key, (int)gameQuests.ElementAt(i).Value.Data.state);
        }
    }
}