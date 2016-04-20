using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SpaceGraph : MonoBehaviour
{
    const int MAX_RESULTS_COUNT = 10;
    const int distanceLimit = 10;
    const int MAX_NODES = 1000;

    public NodeInstance current;

    public Unit unit;

    int nodeMask;

    public Transform world;

    public List<NodeInstance> nodes = new List<NodeInstance>();
    public List<LinkScript> links = new List<LinkScript>();

    int overlapCount;
    Collider[] overlapResults = new Collider[MAX_RESULTS_COUNT];

    void Awake() {
        nodeMask = LayerMask.GetMask("Node");
        Init();
    }

    void Init() {
        nodes = GetComponentsInChildren<NodeInstance>().ToList();
        links = GetComponentsInChildren<LinkScript>().ToList();
        Debug.LogFormat("Init: {0} nodes", nodes.Count);
        nodes.ForEach(node => node.Init());
    }

    void Start() {
        current = current.node.Instantiate(world);
        Bfs();
    }

    void FixedUpdate() {
        int overlapCount = Physics.OverlapBoxNonAlloc(
            unit.characterController.bounds.center,
            unit.characterController.bounds.size / 2, 
            overlapResults,
            unit.transform.rotation,
            nodeMask,
            QueryTriggerInteraction.Collide
        );
        if (overlapCount >= 1) {
            var nodeInstance = overlapResults[0].GetComponentInParent<NodeInstance>();
            if (nodeInstance != null) {
                if (nodeInstance.distance >= 2) {
                    SwitchNode(nodeInstance);
                }
            }
        }
    }

    private void SwitchNode(NodeInstance node) {
        Debug.Log("Switch node to " + node.GetInstanceID());
        current = node;
        Bfs();
    }

    void OverlapNode(NodeInstance newNode) {
        overlapCount = Physics.OverlapBoxNonAlloc(
            newNode.transform.TransformPoint(newNode.bounds.center),
            0.99f * newNode.bounds.size / 2,
            overlapResults,
            newNode.transform.rotation,
            nodeMask,
            QueryTriggerInteraction.Collide
        );
    }

    void Bfs() {
        nodes.ForEach(node => {
            if (node != current) {
                node.ReturnToPool();
            }
        });
        Queue<NodeInstance> queue = new Queue<NodeInstance>();

        nodes.Clear();
        nodes.Add(current); 
        queue.Enqueue(current);
        current.distance = 0;
        int cnt = 1;
        while (queue.Count > 0) {
            var v = queue.Dequeue();
            if (v.distance == distanceLimit) {
                continue;
            }
            v.links.ForEach(link => {
                if (cnt >= MAX_NODES) {
                    Debug.LogWarning("cnt >= MAX_NODES");
                    return;
                }
                var newNode = link.to.node.Instantiate(link.transform);
                //Debug.LogFormat("linking from {0} by {1} to {2}", v, link, newNode);
                newNode.transform.SetParent(world, worldPositionStays: true);
                OverlapNode(newNode);
                if (overlapCount > 1) {
                    //Debug.LogFormat("overlap: {0}", overlapResults[0].transform.Path());
                    newNode.ReturnToPool();
                } else {
                    //Debug.LogFormat("new node fixed: {0}", newNode);
                    newNode.distance = v.distance + 1;
                    nodes.Add(newNode);
                    queue.Enqueue(newNode);
                    cnt++;
                }
            });
            if (cnt >= MAX_NODES) {
                Debug.LogWarning("cnt >= MAX_NODES");
                return;
            }
        }
    }
}