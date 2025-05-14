/*
    Classe de gestion du systeme de sauvegarde du jeu.
        - Creation d'un fichier local de sauvegarde dans la machine de l'utilisateur.
        - Lire les donnes du fichier et les appliquer aux objets appropries du jeu.

    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 11/05/2025
*/  


using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;

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
            Debug.LogError("A SaveManager instance already exists");
        }
        Instance = this;

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

    private void Start()
    {
        string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), pathFromDir);
        fileHandler = new (fullPath, fileName);
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

        if (gameData == null)
        {
            Debug.Log("Couldnt find data making new Data");
            NewGame();
            SaveGame();
            return;
        }

        foreach (IDataSaveable dataSaveable in saveDatas)
        {
            dataSaveable.LoadData(gameData);
        }
    }
    
    public void SaveGame()
    {
        Debug.Log("Saving Game");
        foreach (IDataSaveable dataSaveable in saveDatas)
        {
            dataSaveable.SaveData(ref gameData);
        }

        fileHandler.SaveGameData(gameData);
        Debug.Log("Game has been saved !");
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

    // private void OnApplicationQuit()
    // {
    //     SaveGame();
    // } 
}