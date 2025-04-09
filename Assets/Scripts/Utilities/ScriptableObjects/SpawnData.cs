using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Scriptable Object pour la gestion des donnes des spawns et
    patrouilles des ennemis.    
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 25/03/2025;
*/

[CreateAssetMenu(fileName = "Spawn Data", menuName = "Custom Scriptable Objects/SpawnData"), Serializable]
public class LvlSpawnData : ScriptableObject
{
    public Scene sceneData;
    [field: SerializeField] private string lvlName;
    public GameObject levelSpawn;
    public GameObject[] groups;
    public GameObject[] patrols;

    public void DataScene()
    {
        if (sceneData == null)
        {
            sceneData = SceneManager.GetActiveScene();
        }
    }

    public void UpdateData()
    {
        Transform spawnTrans = levelSpawn.transform;
        groups = new GameObject[spawnTrans.childCount];

        for (int i = 0; i < groups.Length; i++)
        {
            groups[i] = spawnTrans.GetChild(i).gameObject;
        }

        Debug.Log($"There are currently {groups.Length} groups in the scene");
    }
}