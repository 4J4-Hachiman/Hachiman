/*
    Scriptable Object pour les donnes des quetes
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
[CreateAssetMenu(fileName = "SwordsData", menuName = "Custom Scriptable Object/SwordData")]
public class SwordData : ScriptableObject
{
    public SerializableKeyValues<string, GameObject> Katanas { get; private set; }
    [field: SerializeField] public List<Sword> KatanaList { get; private set; }
    
    void OnValidate()
    {
#if UNITY_EDITOR
        for (int i = 0; i < KatanaList.Count; i++)
        {
            Katanas.SetPair(KatanaList[i].ID, KatanaList[i].gameObject);
        }
#endif
    }
}