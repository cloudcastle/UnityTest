using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Blink : Highlight
{
    public float period = 0.25f;
    public float lightPart = 0.5f;

    public bool highlighted;
    public float phase;

    protected virtual void Update() {
        phase = (Time.time % period) / period;
        highlighted = phase < lightPart;
        Switch(highlighted);
    }
}