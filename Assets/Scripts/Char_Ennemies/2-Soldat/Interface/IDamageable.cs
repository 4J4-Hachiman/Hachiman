
using UnityEngine;

public interface IDamageable
{
    float HpCurrent { get; set;}
    float HpMax { get; set;}
    CapsuleCollider CapsuleCollider { get; set; }
    void Dommage(float dmgValeur);
}