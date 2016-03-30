using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Colored))]
public class Key : Item
{
    KeyColor oldKeyColor;
    public KeyColor keyColor;

    Colored colored;

    void Awake() {
        colored = GetComponent<Colored>();
    }

    void Start() {
        if (!Extensions.Editor()) {
            keyColor.keys.Add(this);
        }
    }

    void Update() {
        colored.color = keyColor.color;
    }

    public override void Pick(Activator activator) {
        base.Pick(activator);
        keyColor.doors.ForEach(door => door.OpenFor(activator.player));
    }

    public override void Throw(Player thrower) {
        base.Throw(thrower);
        keyColor.doors.ForEach(door => door.OpenFor(thrower, false));
    }
}