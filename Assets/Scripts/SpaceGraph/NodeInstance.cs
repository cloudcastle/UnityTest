using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NodeInstance : Poolable
{
    public Node node = null;

    public BoxCollider bounds;

    public List<LinkScript> links;

    public int distance;

    public void CreateNode() {
        node = new Node(this);
        gameObject.SetActive(false);
    }

    public void Bfs(int distance = 0) {
    }

    public void Init() {
        links = GetComponentsInChildren<LinkScript>().ToList();
        CreateNode();
    }
}