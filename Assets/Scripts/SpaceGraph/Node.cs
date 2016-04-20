using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Node
{
    static int lastID = 0;
    public string name;

    public Pool pool;

    public Node(NodeInstance sample) {
        this.pool = new Pool(sample.gameObject);
        ++lastID;
        name = sample.name + " #" + lastID;
    }

    public NodeInstance Instantiate(Transform parent) {
        GameObject instance = pool.Take();
        instance.transform.SetParent(parent);
        instance.transform.Reset();
        var nodeInstance = instance.GetComponent<NodeInstance>();
        nodeInstance.node = this;
        return nodeInstance;
    }
}