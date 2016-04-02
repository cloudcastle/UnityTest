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
#if UNITY_EDITOR
        if (!Extensions.Editor()) {
#endif
            keyColor.keys.Add(this);
#if UNITY_EDITOR
        }
#endif
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