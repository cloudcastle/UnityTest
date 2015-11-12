using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightUnderSight : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Color underSightEmission = new Color(0.4f, 0.4f, 0.4f);
    Color baseEmission = Color.black;

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    bool UnderSight() {
        return SightRayCaster.instance.underSight == gameObject;
    }

    void Update() {
        meshRenderer.material.SetColor("_EmissionColor", UnderSight() ? underSightEmission : baseEmission);
    }
}