using UnityEngine;

[ExecuteInEditMode]
public class Sphere : MonoBehaviour
{
    public Color color;

    MeshRenderer meshRenderer;

    void OnEnable() {
        meshRenderer = GetComponent<MeshRenderer>();    
    }

    void Update() {
        var tempMaterial = new Material(meshRenderer.sharedMaterial);
        tempMaterial.color = color;
        meshRenderer.sharedMaterial = tempMaterial;
    }
}
