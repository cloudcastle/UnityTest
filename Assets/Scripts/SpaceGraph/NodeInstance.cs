using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeInstance : MonoBehaviour
{
    public Node node;

    List<NodeInstance> links = new List<NodeInstance>();

    public void CreateNode() {
        node = new Node(this);
    }

    public void Dfs(int distance = 0) {
    }

    public void PreprocessConnectivityComponent() {
    }
}