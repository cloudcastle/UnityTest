using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public static class Overlapper
{
    const int MAX_RESULTS_COUNT = 10;

    static int nodeMask;

    static int overlapCount;
    static Collider[] overlapResults = new Collider[MAX_RESULTS_COUNT];

    static bool inited = false;

    static void CheckOverlapNode(NodeInstance node, float reduction) {        
        //Debug.LogFormat(
        //    "Physics.OverlapBoxNonAlloc({0}, {1}, {2}, {3}, {4}, {5})",
        //    newNode.bounds.transform.TransformPoint(newNode.bounds.center),
        //    reduction * newNode.bounds.size / 2,
        //    overlapResults,
        //    newNode.bounds.transform.rotation,
        //    nodeMask,
        //    QueryTriggerInteraction.Collide
        //);
        overlapCount = Physics.OverlapBoxNonAlloc(
            node.bounds.transform.TransformPoint(node.bounds.center),
            reduction * node.bounds.size / 2,
            overlapResults,
            node.bounds.transform.rotation,
            nodeMask,
            QueryTriggerInteraction.Collide
        ); 
    }

    public static NodeInstance OverlapNode(NodeInstance newNode, float reduction = 0.99f) {
        InitSearching();
        CheckOverlapNode(newNode, reduction);
        return FirstOnNode();
    }

    static NodeInstance FirstOnNode() {
        for (int i = 0; i < overlapCount; i++) {
            if (!overlapResults[i].gameObject.activeInHierarchy) {
                continue;
            }
            var node = overlapResults[i].GetComponentInParent<NodeInstance>();
            if (node == null) {
                Debug.LogFormat("overlap without node: {0}", overlapResults[i].transform.Path());
                Debug.LogFormat("active: {0}", overlapResults[i].gameObject.activeInHierarchy);
            }
            if (node.IsOn()) {
                return node;
            }
        }
        return null;
    }

    static NodeInstance OnlyOnNode() {
        NodeInstance result = null;
        for (int i = 0; i < overlapCount; i++) {
            var node = overlapResults[i].GetComponentInParent<NodeInstance>();
            if (node.IsOn()) {
                if (result != null) {
                    return null;
                }
                result = node;
            } else {
            }
        }
        return result;
    }

    public static NodeInstance OverlapPoint(Vector3 point) {
        InitSearching();
        overlapCount = Physics.OverlapBoxNonAlloc(
            point,
            0.01f * Vector3.one,
            overlapResults,
            Quaternion.identity,
            nodeMask,
            QueryTriggerInteraction.Collide
        );
        return FirstOnNode();
    }

    public static void InitSearching() {
        if (inited) {
            return;
        }
        Debug.LogFormat("Init searching");
        inited = true;
        nodeMask = LayerMask.GetMask("Node");
        overlapResults = new Collider[MAX_RESULTS_COUNT];
    }

    public static NodeInstance FindClosestNode(Unit unit) {
        InitSearching();
        overlapCount = Physics.OverlapBoxNonAlloc(
            unit.eye.transform.position,
            Vector3.zero,
            overlapResults,
            Quaternion.identity,
            nodeMask,
            QueryTriggerInteraction.Collide
        );
        return OnlyOnNode();
    }

#if UNITY_EDITOR
    public static List<NodeInstance> AllOverlapNodes(NodeInstance node, float reduction) {
        CheckOverlapNode(node, reduction);
        List<NodeInstance> result = new List<NodeInstance>();
                    for (int i = 0; i < overlapCount; i++) {
                var overlap = overlapResults[i];
                var other = overlap.GetComponentInParent<NodeInstance>();
                if (other != null) {
                    result.Add(other);
                }
            }
        return result;
    }
#endif
}