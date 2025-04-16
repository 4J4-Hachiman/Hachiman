using System;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    // private bool isFinished = false;
    
    protected void FinishQuestStep()
    {
        GameEvents.TrigOnQuestStepFinished();
        Destroy(gameObject);
    }
}
