/*
    Class regroupant tout les elements a sauvegarder
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 03/05/2025;
*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string lScene;
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public int playerPotionCount;

    public string activeQuest;
    public int currentQuestStep;
    public SerializableKeyValues<string, int> questStates;
    public SerializableKeyValues<string, bool> spawnGroupsStates;
    public SerializableKeyValues<string, bool> chestsStatesOpen;
    public SerializableKeyValues<string, bool> chestsStatesItemPicked;

    public SerializableKeyValues<string, bool> swordState;

    public GameData()
    {
        lScene = "";

        playerPosition = new Vector3(150, 0, 35);
        playerRotation = Quaternion.identity;
        playerPotionCount = 0;

        activeQuest = "NO_ACTIVE_QUESTS";
        currentQuestStep = 0;
        questStates = new ();
        spawnGroupsStates = new ();
        chestsStatesOpen = new ();
        chestsStatesItemPicked = new ();
        swordState = new ();
    }
}

[System.Serializable]
public class SerializableKeyValues<TKey, TValue> : ISerializationCallbackReceiver
{
    public List<TKey> keys = new List<TKey>();
    public List<TValue> values = new List<TValue>();
    private Dictionary<TKey, TValue> pairs = new Dictionary<TKey, TValue>();
    public SerializableKeyValues()
    {
        keys.Clear();
        values.Clear();
        pairs.Clear();
    }
    public SerializableKeyValues(Dictionary<TKey, TValue> pairs)
    {
        keys = pairs.Keys.ToList();
        values = pairs.Values.ToList();
        this.pairs = pairs;
    }
    public SerializableKeyValues(List<TKey> keys, List<TValue> values)
    {
        this.keys = keys;
        this.values = values;
        pairs.Clear();
        for (int i = 0; i < System.Math.Min(this.keys.Count, this.values.Count); i++)
        {
            pairs.Add(this.keys[i], this.values[i]);
        }
    }
    public SerializableKeyValues(TKey[] keys, TValue[] values)
    {
        for (int i = 0; i < System.Math.Min(keys.Length, values.Length); i++)
        {
            this.keys.Add(keys[i]);
            this.values.Add(values[i]);
            pairs.Add(keys[i], values[i]);
        }
    }
    public void SetPair(KeyValuePair<TKey, TValue> keyValuePair)
    {
        if (pairs.ContainsKey(keyValuePair.Key))
        {
            pairs.Remove(keyValuePair.Key);
        }
        pairs.Add(keyValuePair.Key, keyValuePair.Value);
    }
    public void SetPair(TKey key, TValue value)
    {
        if (pairs.ContainsKey(key))
        {
            pairs.Remove(key);
        }

        pairs.Add(key, value);
    }
    public TValue GetKey(TKey key, TValue value)
    {
        if(!pairs.ContainsKey(key))
        {
            SetPair(key, value);
        }

        return pairs[key];
    }
    
    public void OnAfterDeserialize()
    {
        pairs = new Dictionary<TKey, TValue>();
        for (int i = 0; i < System.Math.Min(keys.Count, values.Count); i++)
        {
            pairs.Add(keys[i], values[i]);
        }
    }
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> kvp in pairs)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
}