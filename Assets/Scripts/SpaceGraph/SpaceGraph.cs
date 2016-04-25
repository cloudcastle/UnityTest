using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public partial class SpaceGraph : MonoBehaviour
{
    const int distanceLimit = 14;
    const int MAX_NODES = 1000;

    const int maxAllowedNodeDistance = 0;

    public NodeInstance current;

    public Unit unit;

    public Transform schema;
    public Transform world;

    public List<NodeInstance> nodes = new List<NodeInstance>();
    public List<LinkScript> links = new List<LinkScript>();

    int bfsNodeCount;

    void Awake() {
        if (current == null) {
            current = Overlapper.FindClosestNode(unit);
            Debug.LogFormat("Current was null. Changed to closest node: {0}", current);
        }
        Init();
    }

    void Init() {
        nodes = GetComponentsInChildren<NodeInstance>().ToList();
        links = GetComponentsInChildren<LinkScript>().ToList();
        nodes.ForEach(node => node.On());
        nodes.ForEach(node => node.CreateNode());
        links.ForEach(link => link.CreateLink());
        links.ForEach(link => link.SetBackLink());
        nodes.ForEach(node => node.SetLinks());
        nodes.ForEach(node => node.Off());
        schema.gameObject.SetActive(false);
    }

    void Start() {
        if (current == null) {
            return;
        }
        current = current.node.NewNodeInstance();
        current.On();
        current.transform.SetParent(current.node.transform);
        current.transform.Reset();
        current.transform.SetParent(world, worldPositionStays: true);
        nodes.Clear();
        Bfs();

        FindObjectsOfType<NodeInstance>().Where(ni => ni.links == null).ToList().ForEach(ni => {
            Debug.LogFormat("links == null for {0}", ni);
        });
    }

    void FixedUpdate() {
        var nodeInstance = Overlapper.FindClosestNode(unit);
        if (nodeInstance != null) {
            if (nodeInstance.distance > maxAllowedNodeDistance) {
                SwitchNode(nodeInstance);
            }
        }
    }

    private void SwitchNode(NodeInstance node) {
        current = node;
        //Debug.LogFormat("current = {0}", node);
        Bfs();

        FindObjectsOfType<LinkScript>().ToList().ForEach(link => {
            if (link.to != null) {
                link.AssertAcceptable();
            }
        });
    }

    public static bool Acceptable(NodeInstance node, LinkScript link) {
        var closeTranform = Extensions.Close(node.transform, link.transform);
        return node.node == link.Node && closeTranform;
    }

    NodeInstance CreateNewNode(LinkScript link) {
        var newNode = link.link.to.NewNodeInstance();
        //Debug.LogFormat("New node created for {0}: {1}", link.transform.Path(), newNode);
        newNode.transform.SetParent(link.transform);
        newNode.transform.Reset();
        newNode.transform.SetParent(world, worldPositionStays: true);
        return newNode;
    }

    bool TooFar(NodeInstance node) {
        return node.distance == distanceLimit;
    }

    Queue<NodeInstance> InitBfs() {
        nodes.ForEach(node => node.Off());
        nodes.Clear();
        nodes.Add(current);
        current.On();
        Queue<NodeInstance> queue = new Queue<NodeInstance>();
        queue.Enqueue(current);
        current.distance = 0;
        bfsNodeCount = 1;
        return queue;
    }

    void CancelNode(NodeInstance node) {
    }

    void HandleOverlap(NodeInstance from, NodeInstance overlap, LinkScript link) {
        var overlappedNode = overlap;
        if (Acceptable(overlappedNode, link)) {
            link.to = overlappedNode;
            overlappedNode.links[link.link.backLink].to = from;
        }
    }

    void FollowLink(Queue<NodeInstance> queue, NodeInstance from, LinkScript link) {
        var debug = false;
        //debug = from.name == "Node 8 (4) #2" && link.name == "Up";
        if (debug) {
            Debug.LogFormat("debug link: {0}", link.transform.Path());
        }

        if (bfsNodeCount >= MAX_NODES) {
            Debug.LogWarning("cnt >= MAX_NODES");
            return;
        }

        NodeInstance overlapNode = null;
        overlapNode = Overlapper.OverlapPoint(link.transform.position);
        if (overlapNode != null) {
            HandleOverlap(from, overlapNode, link);
            if (debug) {
                Debug.LogFormat("link {0}: overlapped by point with node {1}", link.transform.Path(), overlapNode);
            }
            return;
        }

        NodeInstance node = null;
        var oldNode = link.to;
        bool oldNodeUsed = false;

        if (oldNode != null) {
            if (oldNode.IsOn()) {
                if (debug) {
                    Debug.LogFormat("link {0}: oldNode is on", link.transform.Path());
                }
                link.AssertAcceptable();
                return;
            } else {
                node = oldNode;
                oldNodeUsed = true;
            }
        } else {
            node = CreateNewNode(link);
            node.Off();
        }

        overlapNode = Overlapper.OverlapNode(node, reduction: 0.99f);
        if (overlapNode != null) {
            if (debug) {
                Debug.LogFormat("link {0}: overlapped with node: {1}", link.transform.Path(), overlapNode);
            }
            node.Disconnect();
            node.ReturnToPoolLight();
            HandleOverlap(from, overlapNode, link);
        } else {
            if (debug) {
                Debug.LogFormat("link {0}: set new node: {1}", link.transform.Path(), node);
            }
            node.On();
            node.distance = from.distance + 1;
            link.to = node;
            var nodeLinks = node.links;
            var backLinkSignature = link.link.backLink;
            var backLink = nodeLinks[backLinkSignature];
            backLink.to = from;
            if (!link.AssertAcceptable()) {
                Debug.LogFormat("oldnode = {0}", oldNodeUsed);
            }
            backLink.AssertAcceptable();
            nodes.Add(node);
            queue.Enqueue(node);
            bfsNodeCount++;
        }
    }

    void BfsRunQueue(Queue<NodeInstance> queue) {
        while (queue.Count > 0) {
            var v = queue.Dequeue();
            if (TooFar(v)) {
                continue;
            }
            v.linksList.ForEach(link => {
                FollowLink(queue, v, link);
            });
            if (bfsNodeCount >= MAX_NODES) {
                Debug.LogWarning("cnt >= MAX_NODES");
                return;
            }
        }
    }

    void Bfs() {
        var oldNodes = nodes.ShallowClone();

        var queue = InitBfs();

        BfsRunQueue(queue);

        oldNodes.ForEach(n => {
            if (!n.IsOn() && n.exists) { 
                n.Disconnect();
                n.ReturnToPool();
            }
        });

        FindObjectsOfType<Pool>().ToList().ForEach(pool => pool.Stabilize());
    }
}