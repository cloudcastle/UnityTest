using UnityEngine;

[ExecuteInEditMode]
public class Sphere : Item
{
    public Color color;
    public Material material;

    MeshRenderer meshRenderer;

    void OnEnable() {
        meshRenderer = GetComponent<MeshRenderer>();    
    }

    void Update() {
        if (material == null) {
            material = meshRenderer.sharedMaterial;
        }
        if (meshRenderer.sharedMaterial == null || meshRenderer.sharedMaterial.color != color || meshRenderer.sharedMaterial.name != material.name) {
            var tempMaterial = new Material(material);
            tempMaterial.color = color;
            meshRenderer.sharedMaterial = tempMaterial;
        }
    }
}
