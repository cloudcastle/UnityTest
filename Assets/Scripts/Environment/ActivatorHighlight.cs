using UnityEngine;

[RequireComponent(typeof(Activatable))]
public class ActivatorHighlight : Blink
{
    public float lightPartOutOfRange = 0.5f;

    Activatable activatable;

    protected override void Awake() {
        base.Awake();
        activatable = GetComponent<Activatable>();
    }

    bool UnderActivator() {
        return Activator.instance.current == activatable;
    }

    bool OutOfRange() {
        return Activator.instance.outOfRange == activatable;
    }

    protected override void Update() {
        if (UnderActivator()) {
            lightPart = 1;
        } else if (OutOfRange()) {
            lightPart = lightPartOutOfRange;
        } else {
            lightPart = 0;
        }
        base.Update();
    }
}