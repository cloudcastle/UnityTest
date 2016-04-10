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
        onPick += keyColor.Recalculate;
        onLose += keyColor.Recalculate;
    }

    void Start() {
        if (!Extensions.Editor()) {
            keyColor.keys.Add(this);
        }
    }

    void Update() {
        if (Extensions.Editor()) {
            if (keyColor == null) {
                keyColor = FindObjectOfType<KeyColor>();
            }
        }
        colored.color = keyColor.color;
    }
}