/*
    Interface pour entites qui bougents
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 13/05/2025;
*/

using UnityEngine;
using UnityEngine.AI;

public interface IMoveable
{
    NavMeshAgent Agent { get; set;}
    void SetNavDestination(Vector3 position);
    void SetNavVitesse(float vitesse);
}
