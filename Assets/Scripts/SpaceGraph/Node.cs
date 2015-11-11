using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node
{
    Pool pool;

    public Node(NodeInstance sample) {
        this.pool = new Pool(sample.gameObject);
    }

    public NodeInstance Instantiate(Transform parent) {
        GameObject instance = pool.Take();
        instance.transform.SetParent(parent);
        return instance.GetComponent<NodeInstance>();
    }
}