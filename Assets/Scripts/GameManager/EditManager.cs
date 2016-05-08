using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class EditManager : MonoBehaviour
{
    public bool on = false;

    RaycastHit hit;

    void Start() {
        Enable(on);
    }

    void Enable(bool on) {
        Debug.Log(string.Format("Editing {0}", on ? "on" : "off"));
        this.on = on;
    }

    void Update() {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F9)) {
            Enable(!on);
        }
        if (on) {
            if (Input.GetMouseButtonDown(0)) {
                var player = Player.instance.current;
                Debug.LogFormat("Raycast through portals by {0}", player);
                var eye = player.eye.transform;
                PortalNode portalNode = null;
                Portal.Raycast(new Ray(eye.position, eye.TransformDirection(Vector3.forward)), out hit, portalSurface => {
                    Debug.LogFormat("Teleported by {0}", portalSurface.transform.Path());
                    if (portalNode == null) {
                        portalNode = portalSurface.portalNode;
                    } else {
                        var child = portalNode.children.FirstOrDefault(pn => pn.surface == portalSurface);
                        if (child == null) {
                            var childObject = new GameObject(portalSurface.portal.name);
                            childObject.transform.SetParent(portalNode.transform);
                            child = childObject.AddComponent<PortalNode>();
                            child.surface = portalSurface;
                            portalNode.children.Add(child);
                            Debug.LogFormat("Created new portal node: {0}", child.transform.Path());
                        }
                        portalNode = child;
                    }
                });
            }
        }
    }
}