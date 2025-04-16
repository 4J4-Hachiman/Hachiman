using UnityEngine;

public class Quest
{
    public QuestData Data { get; private set; }
    int currentStepIndex;
    private GameObject[] questStepGO;

    public Quest(QuestData data)
    {
        Data = data;
        currentStepIndex = 0;

        questStepGO = new GameObject[Data.QuestStepGO.Length];
        for (int i = 0; i < Data.QuestStepGO.Length; i++)
        {
            questStepGO[i] = Data.QuestStepGO[i];
        }
    }

    public void QuestStart()
    {
        Debug.Log("Starting Quest");

        IterateQuestGO(null);
        GameEvents.OnQuestStepFinished += IncrementStep;
    }

    public void IncrementStep()
    {

        currentStepIndex++;
        if (currentStepIndex == Data.QuestStepGO.Length)
        {
            QuestOver();
            return;
        }
        
        Debug.Log("Going to next step");
        IterateQuestGO(null);
    }

    public GameObject GetStepGO()
    {
        return Data.QuestStepGO[currentStepIndex];
    }

    public void IterateQuestGO(Transform parent)
    {
        GameObject questGO = GetStepGO();
        Object.Instantiate(questGO, parent);
    }

    public void QuestOver()
    {
        Debug.Log("The current quest is over");
    }
}