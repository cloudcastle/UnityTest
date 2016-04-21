using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Pool : MonoBehaviour
{
    public GameObject sample;
    int lastInstanceID = 0;

    public List<Poolable> pool = new List<Poolable>(); 

    public static Pool CreatePool(GameObject sample) {
        var newPoolObject = new GameObject();
        newPoolObject.transform.SetParent(PoolManager.instance.transform);
        var newPool = newPoolObject.AddComponent<Pool>();
        newPool.name = sample.name;
        newPool.sample = sample;
        Poolable poolable = sample.GetComponent<Poolable>();
        if (poolable == null) {
            poolable = sample.AddComponent<Poolable>();
        }
        poolable.pool = newPool;
        return newPool;
    }

    void Disappear(Poolable instance) {
        instance.appeared = false;
        instance.transform.SetParent(transform, worldPositionStays: false);
        instance.gameObject.SetActive(false);
    }

    void Appear(Poolable instance) {
        instance.appeared = true;
        instance.gameObject.SetActive(true);
    }

    public void Return(Poolable instance) {
        Disappear(instance);
        pool.Add(instance);
    }

    public GameObject Take() {
        if (pool.Count == 0) {
            ExpandPool();
        }
        Poolable instance = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);
        Appear(instance);
        instance.GetComponents<Script>().ToList().ForEach(script => script.Taken());
        return instance.gameObject;
    }

    void ExpandPool() {
        GameObject instance = GameObject.Instantiate(sample);
        lastInstanceID++;
        instance.name = sample.name + " #" + lastInstanceID;
        var poolable = instance.GetComponent<Poolable>();
        poolable.pool = this;
        pool.Add(poolable);
    }

    public override string ToString() {
        return name;
    }
}