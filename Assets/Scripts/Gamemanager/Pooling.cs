using System.Collections.Generic;
using UnityEngine;

/*
    Class de gestion de pools
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 29/03/2025;
*/


[System.Serializable]
public class Pooling
{
    public readonly GameObject instance;
    public Queue<GameObject> pool;
    private readonly int count;

    public Pooling(GameObject instance, int count)
    {
        this.instance = instance;
        this.count = count;

        pool = new Queue<GameObject>();

        for (int i = 0; i < this.count; i++)
        {
            GameObject newObj = Object.Instantiate(this.instance);
            newObj.SetActive(false);
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
