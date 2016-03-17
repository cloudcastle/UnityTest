using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Highlight : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Color underSightEmission = new Color(0.4f, 0.4f, 0.4f);
    Color baseEmission = Color.black;

    protected virtual void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Switch(bool on) {
        meshRenderer.material.SetColor("_EmissionColor", on ? underSightEmission : baseEmission);
    }
}