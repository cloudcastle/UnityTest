using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;

[Serializable]
public class EditSequence
{
    public List<PortalNodeCreation> commands = new List<PortalNodeCreation>();
}

[Serializable]
public abstract class Command
{
    public virtual void Execute() {
        if (!EditorApplication.isPlaying) {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}

[Serializable]
public class PortalNodeCreation : Command
{
    public string parentPath;
    public string portalSurfacePath;

    public PortalNodeCreation(string parentPath, string portalSurfacePath) {
        this.parentPath = parentPath;
        this.portalSurfacePath = portalSurfacePath;
    }

    public override void Execute() {
        base.Execute();
        var portalSurface = GameObject.Find(portalSurfacePath).GetComponent<PortalSurface>();
        var parent = GameObject.Find(parentPath).GetComponent<PortalNode>();
        var childObject = new GameObject(portalSurface.portal.name);
        childObject.transform.SetParent(parent.transform);
        var child = childObject.AddComponent<PortalNode>();
        child.surface = portalSurface;
        Debug.LogFormat("Created new portal node: {0}", child.transform.Path());
        parent.children.Add(child);
    }
}