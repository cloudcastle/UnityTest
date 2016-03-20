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
        activatable = GetComponent<Activatable>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    bool UnderActivator() {
        return Activator.instance.current == activatable;
    }

    bool OutOfRange() {
        return Activator.instance.outOfRange == activatable;
    }

    bool Ready() {
        return activatable.Ready();
    }

    protected void Update() {
        if (!Ready()) {
            SetEmission(notReadyEmission);
        } else if (UnderActivator()) {
            SetEmission(highlightEmission);
        } else {
            SetEmission(baseEmission);
        }
    }
}