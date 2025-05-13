/*
    Class pour la gestion de points de sauvegarde dans le jeu
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 12/05/2025;
*/

using System;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public Action<SavePoint> OnSavePoint;
    private SphereCollider sphereCollider;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        OnSavePoint?.Invoke(this);
    }
}