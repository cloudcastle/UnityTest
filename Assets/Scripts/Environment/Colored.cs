using UnityEngine;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class Colored : MonoBehaviour
{
    public Color color;
    public Color emissionColor;
    public bool setEmissionColor = false;
    public Material material;

    MeshRenderer meshRenderer;

    void OnEnable() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Update() {
        if (!enabled) {
            return;
        }
        if (Extensions.Editor()) {
            if (material == null) {
                material = meshRenderer.sharedMaterial;
            }
            if (meshRenderer.sharedMaterial == null || meshRenderer.sharedMaterial.color != color) {
                UpdateRendererMaterial();
            }
        } 
    }

    [ContextMenu("Update renderer material")]
    void UpdateRendererMaterial() {
        var tempMaterial = new Material(material);
        tempMaterial.color = color;
        if (setEmissionColor) {
            tempMaterial.SetColor("_EmissionColor", emissionColor);
        }
        meshRenderer.sharedMaterial = tempMaterial;
    }
}
