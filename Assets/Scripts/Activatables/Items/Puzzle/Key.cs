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
        if (!Extensions.Editor()) {
            onPick += keyColor.Recalculate;
            onLose += keyColor.Recalculate;
        }
    }

    void Start() {
        if (!Extensions.Editor()) {
            keyColor.keys.Add(this);
        }
    }

    void Update() {
        if (Extensions.Editor()) {
            if (keyColor == null) {
                Debug.Log("Key {0}: keyColor missing; set some", this);
                keyColor = FindObjectOfType<KeyColor>();
            }
        }
        colored.color = keyColor.color;
        colored.Update();
    }
}