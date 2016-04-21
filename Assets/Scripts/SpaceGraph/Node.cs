using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Node : MonoBehaviour
{
    public Pool pool = null;

    public Transform linkParent;

    public List<Link> links;

    public NodeInstance Instantiate(Transform parent) {
        GameObject instance = pool.Take();
        instance.transform.SetParent(parent);
        instance.transform.Reset();
        var nodeInstance = instance.GetComponent<NodeInstance>();
        nodeInstance.InitLinks();
        nodeInstance.Off();
        return nodeInstance;
    }
}