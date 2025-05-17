/* 
    Classe de base pour les armes du jeu

    Fonctionalités présentes :
        - Dommages de base.
        - Chances de dégats critiques.
        - Multiplicateur de dégats critique.

        - Attaque avec proprietes speciales

    ********************************************************************
    Par : Yanis Oulmane;
    Derniere modification : 13/05/2025;
*/

using UnityEngine;

public class Sword : MonoBehaviour
{
    public enum AttackStats
    {
        None,
        Lifesteal,
        Bleedout
    }

    [field: SerializeField] public string ID { get; private set; }
    
    [Header("Stats")]
    [field: SerializeField] public float BasicDammage { get; private set; } = 100;
    [field: SerializeField] public float HeavyDammage { get; private set; } = 200;
    [field: SerializeField] public float CritChance { get; private set; } = 0;
    [field: SerializeField] public float CritDamage { get; private set; } = 1;
    [field: SerializeField] public AttackStats StatType { get; private set; }
    

    [Header("Layer detection")]
    [field: SerializeField] private LayerMask layerDetection;

    void Awake()
    {
        if (TryGetComponent(out Collider collider))
        {
            collider.includeLayers = layerDetection;
            collider.excludeLayers = ~layerDetection;
        }
    }

    public float GetDammage()
    {
        return CritChance > Random.Range(1, 100) ? BasicDammage * CritDamage : BasicDammage;
    }  
}