using UnityEngine;
using System.Linq;

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

    public override void Pick(Player player) {
        base.Pick(player);
        keyColor.doors.ForEach(door => door.OpenFor(player));
    }

    public override void Throw(Player thrower) {
        base.Throw(thrower);
        if (!thrower.inventory.items.Any(item => item is Key && (item as Key).keyColor == this.keyColor)) {
            keyColor.doors.ForEach(door => door.OpenFor(thrower, false));
        }
    }
}