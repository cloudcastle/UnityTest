using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class PortalNode : MonoBehaviour
{
    public PortalSurface surface;
    public List<PortalNode> children = new List<PortalNode>();

    void Awake() {
        children = this.GetComponentsInMyChildren<PortalNode>();
    }
}
