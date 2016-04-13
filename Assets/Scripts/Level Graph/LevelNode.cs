using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LevelNode : MonoBehaviour
{
    public TextMesh textMesh;
    public string levelName;
    public bool visible = true;
    new MeshRenderer renderer;

    void OnEnable() {
        this.renderer = GetComponent<MeshRenderer>();
    }

    void Update() {
        if (Extensions.Editor()) {
            gameObject.name = levelName;
            textMesh.text = levelName;
            textMesh.transform.localScale /= textMesh.GetComponent<Renderer>().bounds.extents.magnitude * 2;
            renderer.enabled = visible; 
        }
    }

    [ContextMenu("Select Children")]
    void UpdateLevelSet() {
        if (Extensions.Editor()) {
            Selection.objects = FindObjectsOfType<LevelEdge>().Where(e => e.from == this).Select(e => e.to.gameObject).ToArray();
        }
    }
}