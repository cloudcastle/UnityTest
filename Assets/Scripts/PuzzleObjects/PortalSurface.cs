using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PortalSurface : MonoBehaviour
{
    public Portal portal;

    public static int depth = 0;
    public const int maxDepth = 10;

    public new Renderer renderer;
    public Renderer offsetRenderer;

    public PortalNode portalNode;

    public static PortalNode renderingPortalNode = null;

    public static List<PortalSurface> all = new List<PortalSurface>();

    public static List<PortalSurface> surfacesWithChangedTexture = new List<PortalSurface>();

    public static void RestoreTexturesToDepth(int depth) {
        surfacesWithChangedTexture.ForEach(ps => {
            ps.renderer.material.mainTexture = ps.portal.GetCamera(depth).targetTexture;
        });
        surfacesWithChangedTexture.Clear();
    }

    public void SetDepth(int depth) {
        offsetRenderer.material.mainTexture = renderer.material.mainTexture = portal.GetCamera(depth).targetTexture;
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
        if (depth < maxDepth && child != null) {
            var camera = portal.GetCamera(depth);
            camera.transform.SetParent(Camera.current.transform);
            camera.transform.Reset();
            camera.transform.SetParent(portal.front.transform, worldPositionStays: true);
            camera.transform.SetParent(portal.other.back, worldPositionStays: false);
            depth += 1;
            PortalNode parent = renderingPortalNode;
            renderingPortalNode = child;
            camera.Render();
            DebugManager.cnt++;
            depth -= 1;
            renderingPortalNode = parent;
        }
    }
}
