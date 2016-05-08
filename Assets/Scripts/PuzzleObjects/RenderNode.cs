using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class RenderNode : MonoBehaviour
{
    public PortalSurface surface;
    public List<RenderNode> children = new List<RenderNode>();

    void Awake() {
        children = this.GetComponentsInMyChildren<RenderNode>();
    }
}
