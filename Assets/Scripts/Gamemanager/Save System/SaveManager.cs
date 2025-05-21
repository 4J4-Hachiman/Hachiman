/*
    Classe de gestion du systeme de sauvegarde du jeu.
        - Creation d'un fichier local de sauvegarde dans la machine de l'utilisateur.
        - Lire les donnes du fichier et les appliquer aux objets appropries du jeu.

    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 20/05/2025;
*/

using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [Header("Save Data")]
    [field: SerializeField] private string pathFromDir;
    [field: SerializeField] private string fileName;

    public static SaveManager Instance { get; private set; }
    [field: SerializeField] private List<IDataSaveable> saveDatas;
    private GameData gameData;
    private SaveFileHandler fileHandler;

    [field: SerializeField] private Transform savePointsParent;
    private SavePoint[] savePoints;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("A SaveManager instance already exists");
        }
        Instance = this;
        if (savePointsParent.childCount > 0)
        {
            savePoints = new SavePoint[savePointsParent.childCount];

            for (int i = 0; i < savePoints.Length; i++)
            {
                savePoints[i] = savePointsParent.GetChild(i).GetComponent<SavePoint>();
            }

            foreach (SavePoint point in savePoints)
            {
                point.OnSavePoint += OnSavePoint;
            }
        }
        GameEvents.Reset();
        GameEvents.OnQuestFinished += OnQuestFinished;
    }

    private void OnQuestFinished()
    {
        SaveGame();
    }

    private void Start()
    {
        string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), pathFromDir);
        fileHandler = new(fullPath, fileName);
        saveDatas = new List<IDataSaveable>(FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataSaveable>());
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = fileHandler.LoadGameData();

        if (Gamemanager.newGame)
        {
            gameData = null;
            Gamemanager.newGame = false;
        }

        if (gameData == null)
        {
            NewGame();
            SaveGame();
            LoadGame();
            return;
        }

        foreach (IDataSaveable dataSaveable in saveDatas)
        {
            dataSaveable.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        gameData.lScene = SceneManager.GetActiveScene().name;
        gameData.spawnGroupsStates = new();

        foreach (IDataSaveable dataSaveable in saveDatas)
        {
            dataSaveable.SaveData(ref gameData);
        }
        fileHandler.SaveGameData(gameData);
    }

    private void OnSavePoint(SavePoint pt)
    {
        foreach (SavePoint point in savePoints)
        {
            point.gameObject.SetActive(true);
        }

        pt.gameObject.SetActive(false);
        SaveGame();
    }
}