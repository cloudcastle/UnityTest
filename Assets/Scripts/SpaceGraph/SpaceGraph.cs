using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class SpaceGraph : MonoBehaviour
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

#if UNITY_EDITOR
    bool AcceptableEditor(NodeInstance node, LinkScript link) {
        return node == link.to && Extensions.Close(node.transform, link.transform);
    }

    void LocateBackLink(LinkScript link, bool allowCreate = false) {
        NodeInstance from = link.GetComponentInParent<NodeInstance>();
        NodeInstance to = link.to;
        link.backLink = to.GetComponentsInChildren<LinkScript>().ToList().FirstOrDefault(other => {
            if (other.to != from) {
                return false;
            }
            var testObject = new GameObject("TestObject").transform;
            var otherParent = other.transform.parent;
            testObject.SetParent(to.transform, worldPositionStays: false);
            other.transform.SetParent(testObject, worldPositionStays: true);
            testObject.SetParent(link.transform, worldPositionStays: false);
            var result = Extensions.Close(other.transform, from.transform);
            other.transform.SetParent(otherParent, worldPositionStays: false);
            DestroyImmediate(testObject.gameObject); 
            return result;
        });
        if (link.backLink == null) {
            if (!allowCreate) {
                Debug.LogError(string.Format("No backlink detected: {0} {1}", from.name, link.name));
            } else {
                Debug.LogFormat(string.Format("No backlink detected: {0} {1}", from.name, link.name));
                var backlinkObject = new GameObject(GenerateLinkName(link.to, from) + " (backlink)");
                var backlink = backlinkObject.AddComponent<LinkScript>();
                backlink.to = from;
                backlinkObject.transform.SetParent(from.transform);
                backlinkObject.transform.Reset();
                backlinkObject.transform.SetParent(link.transform, worldPositionStays: true);
                var linkFolder = LinkFolder(link.to);
                backlinkObject.transform.SetParent(linkFolder, worldPositionStays: false);
                link.backLink = backlink;
                Debug.LogFormat("Backlink created: {0} {1} - {2} {3}", from.name, link.name, to.name, link.backLink.name);
                backlink.backLink = link;
                Debug.LogFormat("Backlink set: {0} {1} - {2} {3}", to.name, link.backLink.name, from.name, link.name);
            }
        } else {
            Debug.LogFormat("Backlink set: {0} {1} - {2} {3}", from.name, link.name, to.name, link.backLink.name);
        }
    }

    [ContextMenu("Set Back Links")]
    void SetBackLinks() {
        FindObjectsOfType<LinkScript>().ToList().ForEach(link => LocateBackLink(link));
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    [ContextMenu("Create Back Links")]
    void CreateBackLinks() {
        FindObjectsOfType<LinkScript>().ToList().ForEach(link => LocateBackLink(link, allowCreate: true));
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    [ContextMenu("Destroy All Links")]
    void DestroyAllLinks() {
        FindObjectsOfType<LinkScript>().ToList().ForEach(link => DestroyImmediate(link.gameObject));
    }

    [ContextMenu("Destroy Trivial Links")]
    void DestroyTrivialLinks() {
        FindObjectsOfType<LinkScript>().ToList().ForEach(link => {
            if (AcceptableEditor(link.to, link)) {
                DestroyImmediate(link.gameObject);
            } else {
                Debug.LogFormat("Link considered non-trivial and not destroyed: {0}", link.transform.Path());
            }
        });
    }

    [ContextMenu("Rename All Nodes")]
    void RenameAllNodes() {
        int cnt = 0;
        FindObjectsOfType<NodeInstance>().ToList().ForEach(node => {
            ++cnt;
            node.name = "Node " + cnt;
        });
    }

    [ContextMenu("Set Close Links")]
    void SetCloseLinks() {
        FindObjectsOfType<NodeInstance>().ToList().ForEach(node => {

            var oldLinks = node.transform.GetComponentsInChildren<LinkScript>();

            var overlappedNodes = Overlapper.AllOverlapNodes(node, reduction: 1.1f);

            overlappedNodes.ForEach(overlapNode => {
                if (overlapNode != node && (overlapNode.transform.position - node.transform.position).magnitude < 10.1f) {
                    var oldLink = oldLinks.FirstOrDefault(link => link.to == overlapNode && AcceptableEditor(overlapNode, link));
                    if (oldLink == null) {
                        CreateLink(node, overlapNode);
                    } else {
                        Debug.LogFormat("Old link found: {0}", oldLink);
                    }
                }
            });
        });
        FindObjectsOfType<LinkScript>().ToList().ForEach(link => {
            Debug.LogFormat(link.transform.Path());
        });
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    string GenerateLinkName(NodeInstance node, NodeInstance other) {
        var linkName = new List<string>();
        var delta = other.transform.position - node.transform.position;
        if (delta.x > 0.1) {
            linkName.Add("Right");
        }
        if (delta.x < -0.1) {
            linkName.Add("Left");
        }
        if (delta.y > 0.1) {
            linkName.Add("Up");
        }
        if (delta.y < -0.1) {
            linkName.Add("Down");
        }
        if (delta.z > 0.1) {
            linkName.Add("Far");
        }
        if (delta.z < -0.1) {
            linkName.Add("Near");
        }
        return string.Join(" ", linkName.ToArray());
    }

    Transform LinkFolder(NodeInstance node) {
        var linksFolder = node.transform.Find("Links");
        if (linksFolder == null) {
            linksFolder = new GameObject("Links").transform;
            linksFolder.SetParent(node.transform);
            linksFolder.transform.Reset();
        }
        return linksFolder;
    }

    private void CreateLink(NodeInstance node, NodeInstance other) {
        Debug.LogFormat("CreateLink from {0} to {1}", node, other);
        var linksFolder = LinkFolder(node);
        var link = new GameObject(GenerateLinkName(node, other));
        link.transform.SetParent(other.transform);
        link.transform.Reset();
        link.transform.SetParent(linksFolder, worldPositionStays: true);
        var linkScript = link.AddComponent<LinkScript>();
        linkScript.to = other;
    }

    [ContextMenu("Normalize")]
    void Normalize() {
        DestroyTrivialLinks();
        SetCloseLinks();
        SetBackLinks();
    }
#endif

}