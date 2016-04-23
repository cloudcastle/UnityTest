﻿using UnityEngine;
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

    void InitSearching() {
        nodeMask = LayerMask.GetMask("Node");
        overlapResults = new Collider[MAX_RESULTS_COUNT];
    }

    void Awake() {
        InitSearching();
        if (current == null) {
            current = FindClosestNode();
            Debug.LogFormat("Current was null. Changed to closest node: {0}", current);
        }
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

    NodeInstance FindClosestNode() {
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
                return nodeInstance;
            }
        }
        return null;
    }

    void FixedUpdate() {
        var nodeInstance = FindClosestNode();
        if (nodeInstance != null) {
            if (nodeInstance.distance >= 2) {
                SwitchNode(nodeInstance);
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

    void OverlapNode(NodeInstance newNode, float reduction = 0.99f) {
        overlapCount = Physics.OverlapBoxNonAlloc(
            newNode.transform.TransformPoint(newNode.bounds.center),
            reduction * newNode.bounds.size / 2,
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
        return node.node == link.Node && node.transform.CloseTo(link.transform);
    }

    bool AcceptableEditor(NodeInstance node, LinkScript link) {
        return node == link.to && node.transform.CloseTo(link.transform);
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
            var result = other.transform.CloseTo(from.transform);
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

#if UNITY_EDITOR
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

    [ContextMenu("Set Close Links")]
    void SetCloseLinks() {
        InitSearching();

        FindObjectsOfType<NodeInstance>().ToList().ForEach(node => {

            var oldLinks = node.transform.GetComponentsInChildren<LinkScript>();

            OverlapNode(node, reduction: 1.1f);
            for (int i = 0; i < overlapCount; i++) {
                var overlap = overlapResults[i];
                var other = overlap.GetComponentInParent<NodeInstance>();
                if (other != null && other != node) {
                    var oldLink = oldLinks.FirstOrDefault(link => link.to == other && AcceptableEditor(other, link));
                    if (oldLink == null) {
                        CreateLink(node, other);
                    } else {
                        Debug.LogFormat("Old link found: {0}", oldLink);
                    }
                }
            }
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
        SetCloseLinks();
        SetBackLinks();
    }
#endif

}