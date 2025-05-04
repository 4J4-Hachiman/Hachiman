/*
    Scriptable Object pour les donnes des quetes
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

using UnityEngine;

// [System.Serializable]
[CreateAssetMenu(fileName = "QuestData", menuName = "Custom Scriptable Object/QuestData")]
public class QuestData : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public QuestData NextQuest { get; private set; }
    [field: SerializeField] public GameObject[] QuestStepGO { get; private set; }
    [field: SerializeField] public string[] QuestStepInfo { get; private set; }
    public QuestStates state = QuestStates.Inactive;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}