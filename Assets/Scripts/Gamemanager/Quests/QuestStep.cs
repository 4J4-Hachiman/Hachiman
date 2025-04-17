using System;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    public event Action OnQuestStepOver;
    
    protected void FinishQuestStep()
    {
        Destroy(gameObject);
        GameEvents.TrigOnQuestStepFinished();
    }
}
