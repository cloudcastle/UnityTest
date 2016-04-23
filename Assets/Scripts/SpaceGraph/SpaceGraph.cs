using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class SpaceGraph : MonoBehaviour
{
    const int MAX_RESULTS_COUNT = 10;
    const int distanceLimit = 10;
    const int MAX_NODES = 1000;

    public NodeInstance current;

    public Unit unit;

    int nodeMask;

    public Transform schema;
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
        nodes.ForEach(node => node.CreateNode());
        links.ForEach(link => link.CreateLink());
        links.ForEach(link => link.SetBackLink());
        nodes.ForEach(node => node.SetLinks());
        schema.gameObject.SetActive(false);
    }

    void Start() {
        if (current == null) {
            return;
        }
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

    void OverlapNodeAsIfWithTransform(NodeInstance newNode, Transform transform) {
        overlapCount = Physics.OverlapBoxNonAlloc(
            newNode.transform.TransformPoint(newNode.bounds.center),
            0.99f * newNode.bounds.size / 2,
            overlapResults,
            newNode.transform.rotation,
            nodeMask,
            QueryTriggerInteraction.Collide
        );
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

    void OverlapPoint(Vector3 point) {
        overlapCount = Physics.OverlapBoxNonAlloc(
            point,
            0.01f * Vector3.one,
            overlapResults,
            Quaternion.identity,
            nodeMask,
            QueryTriggerInteraction.Collide
        );
    }

    bool Acceptable(NodeInstance node, LinkScript link) {
        return node.node == link.node && node.transform.CloseTo(link.transform);
    }

    NodeInstance FindNodeByPlace(LinkScript link) {
        OverlapPoint(link.transform.position);
        if (overlapCount > 0) {
            var node = overlapResults[0].GetComponentInParent<NodeInstance>();
            if (Acceptable(node, link)) {
                return node;
            }
        }
        return null;
    }

    NodeInstance FindOldNode(LinkScript link) {
        return link.to;
    }

    NodeInstance CreateNewNode(LinkScript link) {
        var newNode = link.link.to.Instantiate(link.transform);
        //Debug.LogFormat("linking from {0} by {1} to {2}", v, link, newNode);
        newNode.transform.SetParent(world, worldPositionStays: true);
        return newNode;
    }

    NodeInstance GetNode(LinkScript link) {
        var oldNode = FindOldNode(link);
        if (oldNode != null) {
            return oldNode;
        } else {
            return CreateNewNode(link);
        }
    }


    bool TooFar(NodeInstance node) {
        return node.distance == distanceLimit;
    }

    void Bfs() {
        List<NodeInstance> oldNodes = nodes.ShallowClone();
        nodes.ForEach(node => node.Off());
        nodes.Clear();
        nodes.Add(current); 
        current.On();
        Queue<NodeInstance> queue = new Queue<NodeInstance>();
        queue.Enqueue(current);
        current.distance = 0;
        int cnt = 1;

        while (queue.Count > 0) {
            var v = queue.Dequeue();
            if (TooFar(v)) {
                continue;
            }
            v.links.Values.ToList().ForEach(link => {
                if (cnt >= MAX_NODES) {
                    Debug.LogWarning("cnt >= MAX_NODES");
                    return;
                }

                var node = GetNode(link);
                if (node.IsOn()) {
                    return;
                }

                OverlapNode(node);
                if (overlapCount > 0) {
                    //Debug.LogFormat("overlap: {0}", overlapResults[0].transform.Path());
                    node.Disconnect();
                    node.ReturnToPool();
                    var overlappedNode = overlapResults[0].GetComponentInParent<NodeInstance>();
                    if (Acceptable(overlappedNode, link)) {
                        link.to = overlappedNode;
                        overlappedNode.links[link.link.backLink].to = v;
                    }
                } else {
                    //Debug.LogFormat("new node fixed: {0}", node);
                    node.On();
                    node.distance = v.distance + 1;
                    link.to = node;
                    node.links[link.link.backLink].to = v;
                    nodes.Add(node);
                    queue.Enqueue(node);
                    cnt++;
                }
            });
            if (cnt >= MAX_NODES) {
                Debug.LogWarning("cnt >= MAX_NODES");
                return;
            }
        }

        oldNodes.ForEach(node => {
            if (!node.IsOn() && node.appeared) {
                node.Disconnect();
                node.ReturnToPool();
            }
        });
    }

    void LocateBackLink(LinkScript link) {
        NodeInstance from = link.GetComponentInParent<NodeInstance>();
        NodeInstance to = link.to;
        link.backLink = to.GetComponentsInChildren<LinkScript>().ToList().FirstOrDefault(other => {
            if (other.to != from) {
                return false;
            }
            var testObject = new GameObject("TestObject").transform;
            testObject.SetParent(to.transform, worldPositionStays: false);
            other.transform.SetParent(testObject);
            testObject.SetParent(link.transform, worldPositionStays: false);
            var result = other.transform.CloseTo(from.transform);
            other.transform.SetParent(to.transform, worldPositionStays: false);
            DestroyImmediate(testObject.gameObject);
            return result;
        });
        if (link.backLink == null) {
            Debug.LogError(string.Format("No backlink detected: {0} {1}", from.name, link.name));
        } else {
            Debug.LogFormat("Backlink set: {0} {1} - {2} {3}", from.name, link.name, to.name, link.backLink.name);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Set Back Links")]
    void SetBackLinks() {
        FindObjectsOfType<LinkScript>().ToList().ForEach(LocateBackLink);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
#endif

}