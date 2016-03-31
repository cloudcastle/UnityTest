using UnityEngine;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class Colored : MonoBehaviour
{
    public Color color;
    public Material material;

    MeshRenderer meshRenderer;

    void OnEnable() {
        meshRenderer = GetComponent<MeshRenderer>();    
    }

    void Update() {
        if (Extensions.Editor()) {
            if (material == null) {
                material = meshRenderer.sharedMaterial;
            }
            if (meshRenderer.sharedMaterial == null || meshRenderer.sharedMaterial.color != color) {
                Debug.Log(String.Format("Colored {0}: material update", this));
                UpdateRendererMaterial();
            }
        }
    }

    [ContextMenu("Update renderer material")]
    void UpdateRendererMaterial() {
        var tempMaterial = new Material(material);
        tempMaterial.color = color;
        meshRenderer.sharedMaterial = tempMaterial;
    }
}
