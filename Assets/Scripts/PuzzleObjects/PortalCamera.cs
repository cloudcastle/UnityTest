using UnityEngine;
using System.Collections.Generic;
using System;

public class PortalCamera : MonoBehaviour
{
    void OnPreRender() {
        SearchManager.instance.portalSurfaces.ForEach(ps => ps.SetDepth(PortalSurface.depth));
    }
}
