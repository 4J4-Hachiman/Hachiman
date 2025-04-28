/*
    Class abatraite pour les quest steps
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025
*/

using System;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    protected void FinishQuestStep()
    {
        Destroy(gameObject);
        GameEvents.TrigOnQuestStepFinished();
    }
}
