/* 
    Classe de base pour les armes du jeu

    Fonctionalités présentes :
        - Dommages de base.
        - Chances de dégats critiques.
        - Multiplicateur de dégats critique.

    ********************************************************************
    Par : Yanis Oulmane;
    Derniere modification : 12/04/2025;
*/

using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Sword : MonoBehaviour
{
    [Header("Stats")]
    [field: SerializeField] public float BasicDammage { get; private set; } = 100;
    [field: SerializeField] public float HeavyDammage { get; private set; } = 200;
    [field: SerializeField] public float CritChance { get; private set; } = 0;
    [field: SerializeField] public float CritDamage { get; private set; } = 1;
    [field: SerializeField] private LayerMask layerDetection;

    void Awake()
    {
        GetComponent<CapsuleCollider>().includeLayers = layerDetection;
        GetComponent<CapsuleCollider>().excludeLayers = ~layerDetection;
    }
    
    public float GetDammage()
    {
        return CritChance > Random.Range(1, 100) ? BasicDammage * CritDamage : BasicDammage;
    }
}
