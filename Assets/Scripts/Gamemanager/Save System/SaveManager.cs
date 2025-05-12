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

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("A SaveManager instance already exists");
        }
        Instance = this;
    }

    private void Start()
    {
        string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), pathFromDir);
        fileHandler = new (fullPath, fileName);
        saveDatas = new List<IDataSaveable>(FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataSaveable>());
        // LoadGame();
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
        foreach (IDataSaveable dataSaveable in saveDatas)
        {
            dataSaveable.SaveData(ref gameData);
        }

        fileHandler.SaveGameData(gameData);
    }

    private void OnApplicationQuit()
    {
        // Debug.Log("QUIT THE APPLICATION");
        // SaveGame();
    } 
}