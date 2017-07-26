using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PortalSurface : MonoBehaviour
{
    public Portal portal;

    public static int depth = 0;
    public const int maxDepth = 20;

    public new Renderer renderer;
    public Renderer offsetRenderer;

    public Material portalMaterial;
    public Material fogMaterial;

    public PortalNode portalNode;

    public static PortalNode renderingPortalNode = null;

    public void SetDepth(int depth) {
        var camera = portal.GetCamera(depth);
        if (camera.GetComponent<PortalCamera>().rendered) {
            offsetRenderer.material = renderer.material = portalMaterial;
            offsetRenderer.material.mainTexture = renderer.material.mainTexture = camera.targetTexture;
        } else {
            offsetRenderer.material = renderer.material = fogMaterial;
        }
        //if (DebugManager.debug) {
        //    Debug.LogFormat("renderer {0} texture set to {1}", renderer.transform.Path(), portal.GetCamera(depth).targetTexture);
        //}
    }

    void OnWillRenderObject() {
        if (Camera.current == Camera.main) {
            depth = 0;
            renderingPortalNode = null;
        }
        if (Camera.current != Camera.main && depth == 0) { // some camera not main and not rendered recursively, i.e. editor camera
            return;
        }
        PortalNode child = null;
        if (renderingPortalNode == null) {
            child = this.portalNode;
        } else {
            child = renderingPortalNode.children.FirstOrDefault(pn => pn.surface == this);
        }
        var camera = portal.GetCamera(depth);
        if (depth < maxDepth && child != null) {
            camera.transform.SetParent(Camera.current.transform);
            camera.transform.Reset();
            camera.transform.SetParent(portal.front.transform, worldPositionStays: true);
            camera.transform.SetParent(portal.other.back, worldPositionStays: false);
            depth += 1;
            PortalNode parent = renderingPortalNode;
            renderingPortalNode = child;
            camera.Render();
            DebugManager.cnt++;
            DebugManager.drawnPortals.Add(renderingPortalNode.transform.Path());
            depth -= 1;
            renderingPortalNode = parent;
            camera.GetComponent<PortalCamera>().rendered = true;
        } else {
            camera.GetComponent<PortalCamera>().rendered = false;
        }
    }
}
