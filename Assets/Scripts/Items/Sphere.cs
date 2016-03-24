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
        if (meshRenderer.sharedMaterial == null || meshRenderer.sharedMaterial.color != color) {
            Debug.Log("Sphere: material update");
            UpdateRendererMaterial();
        }
    }

    [ContextMenu("Update renderer material")]
    void UpdateRendererMaterial() {
        var tempMaterial = new Material(material);
        tempMaterial.color = color;
        meshRenderer.sharedMaterial = tempMaterial;
    }
}
