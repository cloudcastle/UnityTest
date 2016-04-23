using UnityEngine;

[RequireComponent(typeof(Activatable))]
public class ActivatorHighlight : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public Color highlightEmission = new Color(0.4f, 0.4f, 0.4f);
    public Color baseEmission = Color.black;
    public Color notReadyEmission = new Color(0.7f, 0.7f, 0.7f);

    public void SetEmission(Color emission) {
        meshRenderer.material.SetColor("_EmissionColor", emission);
    }

    public float lightPartOutOfRange = 0.5f;

    Activatable activatable;

    void Awake() {
        activatable = GetComponentInChildren<Activatable>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    bool UnderActivator() {
        return Player.instance.current.activator.current == activatable;
    }

    protected void Update() {
        if (activatable.Status() == ActivatableStatus.Activated) {
            SetEmission(notReadyEmission);
        } else if (activatable.Status() == ActivatableStatus.Activatable && UnderActivator()) {
            SetEmission(highlightEmission);
        } else {
            SetEmission(baseEmission);
        }
    }
}