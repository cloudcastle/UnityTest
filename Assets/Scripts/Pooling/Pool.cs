using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool
{
    GameObject sample;

    List<GameObject> pool = new List<GameObject>();

    public Pool(GameObject sample) {
        this.sample = sample;
        Poolable poolable = sample.AddComponent<Poolable>();
        poolable.pool = this;
        Disappear(sample);
    }

    void Disappear(GameObject instance) {
        instance.transform.parent = PoolManager.instance.transform;
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
        return instance;
    }

    void ExpandPool() {
        GameObject instance = GameObject.Instantiate(sample);
        pool.Add(instance);
    }
}