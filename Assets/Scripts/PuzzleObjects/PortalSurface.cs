using UnityEngine;
using System.Collections.Generic;

public class PortalSurface : MonoBehaviour
{
    public Portal portal;
    public RenderTexture portalTexture;

    public static int depth = 0;
    public const int maxDepth = 2;

    void OnWillRenderObject() {
        if (Camera.current == Camera.main) {
            depth = 0;
        }
        if (Camera.current != Camera.main && depth == 0) {
            return;
        }
        if (depth < maxDepth) {
            var camera = portal.GetCamera(depth);
            camera.transform.SetParent(Camera.current.transform);
            camera.transform.Reset();
            camera.transform.SetParent(portal.front.transform, worldPositionStays: true);
            camera.transform.SetParent(portal.other.back, worldPositionStays: false);
            depth += 1;
            camera.Render();
            depth -= 1;
        }
    }
}
