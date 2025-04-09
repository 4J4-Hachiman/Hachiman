/*
    Class de gestion du combat des ennemis avec le joueur;
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 29/03/2025;
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CombatManager
{
    private readonly Gamemanager gamemanager;
    private readonly float surroundDistance;
    private readonly float enemySpacing;
    private List<Vector3> claimedOffsets;
    private List<EnnemiMain> readyEnemies;
    private event Action OnFirstEnemyReady;
    private EnnemiMain attacker;

    public CombatManager(Gamemanager gamemanager, float surroundDistance, float enemySpacing)
    {
        this.gamemanager = gamemanager;
        this.surroundDistance = surroundDistance;
        this.enemySpacing = enemySpacing;
        claimedOffsets = new List<Vector3>();
        readyEnemies = new List<EnnemiMain>();
        OnFirstEnemyReady += HandleFirstEnemyReady;
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

    /// <summary>Adds an enemy to the list of enemies ready to attack.</summary>
    /// <param name="instance">instance the enemy's EnnemiMain Class.</param>
    public void AddToReadyList(EnnemiMain instance)
    {
        if (readyEnemies.Count == 0)
        {
            OnFirstEnemyReady?.Invoke();
        }
        if (!readyEnemies.Contains(instance))
        {
            readyEnemies.Add(instance);
        }

        Debug.Log($"Added an enemy to the ready list. New count {readyEnemies.Count}");
    }

    /// <summary>Removes an enemy from the list of enemies ready to attack the player.</summary>
    /// <param name="instance">Instance of EnnemiMain that will be removed.</param>
    public void RemoveFromReadyList(EnnemiMain instance)
    {
        readyEnemies.Remove(instance);
        Debug.Log($"Removed an enemy from the ready list. New count {readyEnemies.Count}");
    }

    private void HandleFirstEnemyReady()
    {
        OnFirstEnemyReady -= HandleFirstEnemyReady;
        gamemanager.StartCoroutine(ManageAttack());
    }

    private void HandleAttackPerformed()
    {
        attacker.StateAttack.OnAttackEnd -= HandleAttackPerformed;  
        gamemanager.StartCoroutine(ManageAttack());
    }

    private void HandleEnemyDeath(EnnemiMain instance)
    {
        RemoveFromReadyList(instance);
        instance.OnEnemyDeath -= HandleEnemyDeath;
        gamemanager.StartCoroutine(ManageAttack());
    }
    
    private IEnumerator ManageAttack()
    {
        // Debug.Log($"Started Attack Coroutine with {readyEnemies.Count} enemies ready to attack");
        // If there is no enemy ready, wait for one to be ready
        while (readyEnemies.Count == 0)
        {
            yield return new WaitForEndOfFrame();
        }
        // Debug.Log("Found a enemy ready to attack");

        // Chose a random enemy from the list of enemies ready to attack
        int indexAttacker = UnityEngine.Random.Range(0, readyEnemies.Count);
        attacker = readyEnemies[indexAttacker];
        yield return new WaitForSeconds(readyEnemies.Count == 1 ? 3 : UnityEngine.Random.Range(2, 5));
        attacker.StateAttack.OnAttackEnd += HandleAttackPerformed;
        attacker.OnEnemyDeath += HandleEnemyDeath;
        RemoveFromReadyList(attacker);

        // Wait a few seconds
        if (attacker.StateMachine.currentState != attacker.StateDead)
        {
            // Debug.Log("Triggered an attack");
            attacker.TriggerAttack();
            yield break;
        }

        yield break;
    }

    /// <summary>Generate a random normalized Vector2.</summary>
    /// <returns>Normzalied Vector2.</returns>
    private Vector2 GetRandomVector2()
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = Mathf.Sqrt(1 - Mathf.Pow(x, 2)) * (UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1);
        return new Vector2(x, y).normalized;
    }
}