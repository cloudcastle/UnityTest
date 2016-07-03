using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

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

    #if UNITY_EDITOR
    void PortalRay(Ray ray) {
        PortalNode portalNode = null;
        Portal.Raycast(ray, out hit, portalSurface => {
            if (portalNode == null) {
                portalNode = portalSurface.portalNode;
            } else {
                var child = portalNode.children.FirstOrDefault(pn => pn.surface == portalSurface);
                if (child == null) {
                    var command = new PortalNodeCreation(portalNode.transform.Path(), portalSurface.transform.Path());
                    command.Execute();
                    sequence.commands.Add(command);
                }
                portalNode = child;
            }
        }, mask: ~LayerMask.GetMask("ItemProof"));
    }

    void Update() {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F9)) {
            Enable(!on);
        }
        if (on) {
            if (Input.GetMouseButtonDown(0)) {
                var player = Player.instance.current;
                var eye = player.eye.transform;
                Ray ray = new Ray(eye.position, eye.TransformDirection(Vector3.forward));
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
        Debug.LogFormat("Number of commands: {0}", sequence.commands.Count);
        sequence.commands.ForEach(command => {
            command.Execute();
        });
    }

    [ContextMenu("Remove duplicate portal nodes")]
    void RemoveDuplicateNodes() {
        FindObjectsOfType<PortalNode>().ForEach(pn => {
            var uniqueChildren = new List<PortalNode>();
            pn.children.ForEach(child => {
                if (uniqueChildren.All(uc => uc.surface != child.surface)) {
                    uniqueChildren.Add(child);
                }
            });
            pn.children.Where(c => !uniqueChildren.Contains(c)).ForEach(c => {
                DestroyImmediate(c.gameObject);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            });
        });
    }
    #endif
}