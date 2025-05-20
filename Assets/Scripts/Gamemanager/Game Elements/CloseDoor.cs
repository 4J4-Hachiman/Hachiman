/*
    Ferme porte
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 19/05/2025;
*/

using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [field: SerializeField] private Transform door1;
    [field: SerializeField] private Transform door2;
    [field: SerializeField] private Quaternion rotationD1;
    [field: SerializeField] private Quaternion rotationD2;

    void OnTriggerEnter(Collider other)
    {
        door1.rotation = rotationD1;
        door2.rotation = rotationD2;
        GetComponent<Collider>().enabled = false;
    }
}
