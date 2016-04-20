using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class Pool
{
    static int lastID = 0;
    public string name;

    public GameObject sample;
    int lastInstanceID = 0;

    public List<GameObject> pool = new List<GameObject>();

    public Pool(GameObject sample) {
        ++lastID;
        name = "Pool #" + lastID.ToString();
        this.sample = sample;
        Poolable poolable = sample.GetComponent<Poolable>();
        if (poolable == null) {
            poolable = sample.AddComponent<Poolable>();
        }
        poolable.pool = this;
    }

    void Disappear(GameObject instance) {
        instance.transform.SetParent(PoolManager.instance.transform, worldPositionStays: false);
        instance.SetActive(false);
    }

    void Appear(GameObject instance) {
        instance.SetActive(true);
    }

    public void Return(GameObject instance) {
        Disappear(instance);
        pool.Add(instance);
    }

    public GameObject Take() {
        if (pool.Count == 0) {
            ExpandPool();
        }
        GameObject instance = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);
        Appear(instance);
        instance.GetComponents<Script>().ToList().ForEach(script => script.Taken());
        return instance;
    }

    void ExpandPool() {
        GameObject instance = GameObject.Instantiate(sample);
        lastInstanceID++;
        instance.name = sample.name + " #" + lastInstanceID;
        instance.GetComponent<Poolable>().pool = this;
        pool.Add(instance);
    }

    public override string ToString() {
        return name;
    }
}