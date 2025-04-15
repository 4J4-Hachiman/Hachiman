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
    private readonly float surroundDistance;
    private readonly float enemySpacing;
    private readonly List<Vector3> claimedOffsets;
    private readonly List<EnnemiMain> readyEnemies;
    private EnnemiMain attacker;
    private bool enemyIsAttacking = false;

    public CombatManager(Gamemanager gamemanager, float surroundDistance, float enemySpacing)
    {
        this.gamemanager = gamemanager;
        this.surroundDistance = surroundDistance;
        this.enemySpacing = enemySpacing;
        claimedOffsets = new List<Vector3>();
        readyEnemies = new List<EnnemiMain>();
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
        instance.OnActionOver -= HandleOnActionOver;

        if (!readyEnemies.Contains(instance))
        {
            readyEnemies.Add(instance);
        }

        gamemanager.StartCoroutine(ManageAttack());
    }

    public void RemoveFromReadyList(EnnemiMain instance)
    {
        readyEnemies.Remove(instance);
    }

    private void HandleOnActionOver(EnnemiMain instance)
    {
        enemyIsAttacking = false;
        Debug.Log("<color=purple> Enemy Action is over");
        instance.OnActionOver -= HandleOnActionOver;
        gamemanager.StartCoroutine(ManageAttack());
    }

    private IEnumerator ManageAttack()
    {
        Debug.Log("Started the attack coroutine");
        if (enemyIsAttacking)
        {
            Debug.Log("An enemy is already attacking");
            yield break;
        }

        enemyIsAttacking = true;

        while (readyEnemies.Count == 0)
        {
            Debug.Log("No enemy is ready");
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(readyEnemies.Count == 3 ? 5 : Random.Range(1, 3));

        int indexAttacker = Random.Range(0, readyEnemies.Count);
        attacker = readyEnemies[indexAttacker];
        attacker.OnActionOver += HandleOnActionOver;

        if (attacker.StateMachine.currentState != attacker.StateDead)
        {
            attacker.TriggerAttack();
        }
        else
        {
            Debug.Log("<color=red>The Enemy is already dead");
        }
        yield break;
    }

    private Vector2 GetRandomVector2()
    {
        float x = Random.Range(-1f, 1f);
        float y = Mathf.Sqrt(1 - Mathf.Pow(x, 2)) * (Random.Range(0, 2) == 0 ? 1 : -1);
        return new Vector2(x, y).normalized;
    }
}