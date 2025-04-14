/*
    Class de gestion du combat des ennemis avec le joueur;
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 12/04/2025;
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager
{
    private readonly Gamemanager gamemanager;
    private bool enemyIsAttacking;
    private readonly float surroundDistance;
    private readonly float enemySpacing;
    private List<Vector3> claimedOffsets;
    private List<EnnemiMain> readyEnemies;
    // private event Action OnFirstEnemyReady;
    private EnnemiMain attacker;

    public CombatManager(Gamemanager gamemanager, float surroundDistance, float enemySpacing)
    {
        this.gamemanager = gamemanager;
        this.surroundDistance = surroundDistance;
        this.enemySpacing = enemySpacing;
        claimedOffsets = new List<Vector3>();
        readyEnemies = new List<EnnemiMain>();
        // OnFirstEnemyReady += HandleFirstEnemyReady;
    }

    public Vector3 GetNewSurroundPos()
    {
        int attempts = 0;
        float varRadius = surroundDistance;
        Vector3 offset;
        bool isValid;
        do
        {
            isValid = true;
            Vector2 randCircle = GetRandomVector2();
            offset = new Vector3(randCircle.x, 0, randCircle.y) * varRadius;

            foreach (Vector3 claimedPos in claimedOffsets)
            {
                // Check if position is too close to already claimed one
                if (Vector3.Distance(offset, claimedPos) < enemySpacing)
                {
                    isValid = false;
                    attempts++;

                    if (attempts > 5)
                    {
                        attempts = 0;
                        varRadius += enemySpacing;
                    }
                }
            }
        } while (!isValid);

        claimedOffsets.Add(offset);
        return offset;
    }

    public void AddToReadyList(EnnemiMain instance)
    {
        if (!readyEnemies.Contains(instance))
        {
            if (readyEnemies.Count == 0)
            {
                readyEnemies.Add(instance);
                gamemanager.StartCoroutine(ManageAttack());
            }
            readyEnemies.Add(instance);
        }
        // Debug.Log("Added enemy to ready list");
    }

    public void RemoveFromReadyList(EnnemiMain instance)
    {
        readyEnemies.Remove(instance);
    }

    private void HandleOnActionOver(EnnemiMain instance)
    {
        // Debug.Log("The enemy's action is over restarting Attack Coroutine");
        instance.OnActionOver -= HandleOnActionOver;
        gamemanager.StartCoroutine(ManageAttack());
    }

    private IEnumerator ManageAttack()
    {
        if (enemyIsAttacking)
        {
            yield break;
        }

        enemyIsAttacking = true;
        
        while (readyEnemies.Count == 0)
        {
            yield return new WaitForEndOfFrame();
        }

        // Debug.Log(readyEnemies.Count);
        int indexAttacker = UnityEngine.Random.Range(0, readyEnemies.Count);
        attacker = readyEnemies[indexAttacker];
        yield return new WaitForSeconds(readyEnemies.Count == 3 ? 5 : UnityEngine.Random.Range(1, 3));
        attacker.OnActionOver += HandleOnActionOver;

        if (attacker.StateMachine.currentState != attacker.StateDead)
        {
            // Debug.Log("Triggered an enemy attack");
            attacker.TriggerAttack();
        }

        enemyIsAttacking = false;
        yield break;
    }

    private Vector2 GetRandomVector2()
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = Mathf.Sqrt(1 - Mathf.Pow(x, 2)) * (UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1);
        return new Vector2(x, y).normalized;
    }
}