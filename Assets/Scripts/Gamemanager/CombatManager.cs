/*
    Class de gestion du combat des ennemis avec le joueur;
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/04/2025;
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
    private List<EnnemiMain> fightingEnemies;
    private bool enemyIsAttacking = false;

    public CombatManager(Gamemanager gamemanager, float surroundDistance, float enemySpacing)
    {
        this.gamemanager = gamemanager;
        this.surroundDistance = surroundDistance;
        this.enemySpacing = enemySpacing;
        claimedOffsets = new List<Vector3>();
        readyEnemies = new List<EnnemiMain>();
        fightingEnemies = new List<EnnemiMain>();
    }

    public void StartCombat(List<GameObject> enemyList)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            fightingEnemies.Add(enemyList[i].GetComponent<EnnemiMain>());
            fightingEnemies[i].OnActionOver += HandleOnActionOver;
        }
        gamemanager.StartCoroutine(AttackCoroutine());
    }

    public void EnemyDeath(EnnemiMain instance)
    {
        fightingEnemies.Remove(instance);
    }

    private void HandleOnActionOver(EnnemiMain instance)
    {
        enemyIsAttacking = false;
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(readyEnemies.Count == 1 ? 2 : Random.Range(2, 4));
            int indexAttacker = Random.Range(0, fightingEnemies.Count);
            if (fightingEnemies.Count == 0)
            {
                yield break;
            }
            fightingEnemies[indexAttacker].TriggerAttack();
            enemyIsAttacking = true;
            while (enemyIsAttacking)
            {
                yield return null;
            }
            yield return null;
        }
    }

    public Vector3 GetNewSurroundPos()
    {
        int iterration = 0;
        float vRad = surroundDistance;
        Vector3 offset;
        bool valid;
        do
        {
            valid = true;
            Vector2 randCircle = GetRandV2();
            offset = new Vector3(randCircle.x, 0, randCircle.y) * vRad;

            foreach (Vector3 claimedPos in claimedOffsets)
            {
                if (Vector3.Distance(offset, claimedPos) < enemySpacing)
                {
                    valid = false;
                    iterration++;
                    if (iterration > 5)
                    {
                        iterration = 0;
                        vRad += enemySpacing;
                    }
                }
            }
        } while (!valid);
        claimedOffsets.Add(offset);
        return offset;
    }

    private Vector2 GetRandV2()
    {
        float x = Random.Range(-1f, 1f);
        float y = Mathf.Sqrt(1 - Mathf.Pow(x, 2)) * (Random.Range(0, 2) == 0 ? 1 : -1);
        return new Vector2(x, y).normalized;
    }
}