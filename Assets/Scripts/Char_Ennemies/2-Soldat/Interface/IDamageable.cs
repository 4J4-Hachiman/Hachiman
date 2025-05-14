/*
    Interface pour entite dommagable
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 13/05/2025;
*/

using UnityEngine;

public interface IDamageable
{
    float HpCurrent { get; set;}
    float HpMax { get; set;}
    CapsuleCollider CapsuleCollider { get; set; }
    void Dommage(float dmgValeur, bool dmgFromStat);
}