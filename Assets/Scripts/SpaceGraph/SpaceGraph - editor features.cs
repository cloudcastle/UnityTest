#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public partial class SpaceGraph
{
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
}

#endif