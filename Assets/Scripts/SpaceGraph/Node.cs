using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Node : MonoBehaviour
{
    public Pool pool = null;

    public Transform linkParent;

    public List<Link> links;

    public NodeInstance NewNodeInstance() {
        return pool.Take().GetComponent<NodeInstance>();
    }
}