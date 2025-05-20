/*
    Evenements globaux
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 20/05/2025
*/

using System;

public class GameEvents
{
    public static event Action OnAllEnemiesKilled;
    public static void TrigAllEnemiesKilled() => OnAllEnemiesKilled?.Invoke();
    public static event Action OnLocationReached;
    public static void TrigOnLocationReached() => OnLocationReached?.Invoke();
    public static event Action OnQuestStepFinished;
    public static void TrigOnQuestStepFinished() => OnQuestStepFinished?.Invoke();
    public static event Action OnQuestItemPickedUp;
    public static void TrigOnQuestItemPickedUp() => OnQuestItemPickedUp?.Invoke();
    public static void Reset()
    {
        OnAllEnemiesKilled = null;
        OnLocationReached = null;
        OnQuestStepFinished = null;
        OnQuestItemPickedUp = null;
    }
}