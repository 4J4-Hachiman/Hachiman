/*
    Class de gestion de pools
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 15/04/2025;
*/

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pooling
{
    public readonly GameObject instance;
    public Queue<GameObject> pool;
    public readonly int count;

    public Pooling(GameObject instance, int count, GameObject parent = null)
    {
        this.instance = instance;
        this.count = count;

        pool = new Queue<GameObject>();

        for (int i = 0; i < this.count; i++)
        {
            GameObject newObj = Object.Instantiate(this.instance);
            newObj.SetActive(false);
            if (parent)
            {
                newObj.transform.SetParent(parent.transform);
            }
            pool.Enqueue(newObj);
        }
    }
    
    public GameObject GetFromPool()
    {
        return pool.Dequeue();
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}