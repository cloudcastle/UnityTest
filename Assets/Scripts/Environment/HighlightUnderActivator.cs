using UnityEngine;

[RequireComponent(typeof(Activatable))]
public class HighlightUnderActivator : Highlight
{
    Activatable activatable;

    protected override void Awake() {
        base.Awake();
        activatable = GetComponent<Activatable>();
    }

    bool UnderActivator() {
        return Activator.instance.activatable == activatable;
    }

    void Update() {
        Switch(UnderActivator());
    }
}