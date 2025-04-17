using System;
using UnityEngine;

public class GameEvents
{
    public static event Action OnAllEnemiesKilled;
    public static void TrigAllEnemiesKilled() => OnAllEnemiesKilled?.Invoke();

    public static event Action OnLocationReached;
    public static void TrigOnLocationReached() => OnLocationReached?.Invoke();

    public static event Action OnQuestStepFinished;
    public static void TrigOnQuestStepFinished() => OnQuestStepFinished?.Invoke();
}