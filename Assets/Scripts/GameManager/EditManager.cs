using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class EditManager : MonoBehaviour
{
    const string editSequenceFile = "editSequence.dat";

    public EditSequence sequence = new EditSequence();

    public bool on = false;

    RaycastHit hit;

    void Start() {
        Enable(on);
        sequence = new EditSequence();
    }

    void Enable(bool on) {
        Debug.Log(string.Format("Editing {0}", on ? "on" : "off"));
        this.on = on;
    }

    void PortalRay(Ray ray) {
        PortalNode portalNode = null;
        Portal.Raycast(ray, out hit, portalSurface => {
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

    void Update() {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F9)) {
            Enable(!on);
        }
        if (on) {
            if (Input.GetMouseButtonDown(0)) {
                var player = Player.instance.current;
                Debug.LogFormat("Raycast through portals by {0}", player);
                var eye = player.eye.transform;
                Ray ray = new Ray(eye.position, eye.TransformDirection(Vector3.forward));
                sequence.rays.Add(ray);
                PortalRay(ray);
            }
        }
    }

    void OnDestroy() {
        FileManager.SaveToFile(sequence, editSequenceFile);
        Debug.LogFormat("Edit Manager OnDestroy");
    }

    [ContextMenu("Apply edit sequence")]
    void ApplyEditSequence() {
        EditSequence sequence = FileManager.LoadFromFile<EditSequence>(editSequenceFile);
        Debug.LogFormat("Number of rays: {0}", sequence.rays.Count);
        sequence.rays.ForEach(ray => {
            Debug.LogFormat("Ray {0} {1}", ray.origin.ExtToString(), ray.direction.ExtToString());
        });
    }
}