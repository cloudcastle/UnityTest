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

    public List<Poolable> lightPool = new List<Poolable>();

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
        instance.gameObject.SetActive(false);
        instance.transform.SetParent(transform, worldPositionStays: false);
    }

    void Appear(Poolable instance) {
        instance.gameObject.SetActive(true);
    }

    public void Return(Poolable instance) {
        if (instance.gameObject == sample) {
            Debug.LogError("Sample returning to pool!");
        }
        instance.exists = false;
        Disappear(instance);
        pool.Add(instance);
    }

    public void ReturnLight(Poolable instance) {
        instance.exists = false;
        lightPool.Add(instance);
    }

    public void Stabilize() {
        lightPool.ForEach(p => {
            Disappear(p);
            pool.Add(p);
        });
        lightPool.Clear();
    }

    public GameObject Take() {
        Poolable instance;
        if (lightPool.Count == 0) {
            if (pool.Count == 0) {
                ExpandPool();
            }
            instance = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            Appear(instance);
        } else {
            instance = lightPool[lightPool.Count - 1];
            lightPool.RemoveAt(lightPool.Count - 1);
        }
        instance.GetComponents<Script>().ToList().ForEach(script => script.Taken());
        instance.exists = true;
        return instance.gameObject;
    }

    void ExpandPool() {
        GameObject instance = GameObject.Instantiate(sample);
        lastInstanceID++;
        instance.name = sample.name + " #" + lastInstanceID;
        var poolable = instance.GetComponent<Poolable>();
        poolable.Instantiated();
        poolable.pool = this;
        poolable.exists = false;
        pool.Add(poolable);
    }

    public override string ToString() {
        return name;
    }
}