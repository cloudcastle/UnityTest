using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(Colored))]
[RequireComponent(typeof(Item))]
public class Key : Script
{
    KeyColor oldKeyColor;
    public KeyColor keyColor;

    public Item item;

    Colored colored;
    new Renderer renderer;

    public override void Awake() {
        colored = GetComponent<Colored>();
        renderer = GetComponent<Renderer>();
        item = GetComponent<Item>();
        if (!Extensions.Editor()) {
            base.Awake();
            item.onPick += keyColor.Recalculate;
            item.onLose += keyColor.Recalculate;
        }
    }

    public override void Start() {
        if (!Extensions.Editor()) {
            base.Start();
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

        //if (!Extensions.Editor()) {
        //    gameObject.name = string.Format("{0} key{1}", keyColor.name, inventorySlot == null ? "" : " in " + inventorySlot.name);
        //}
    }
}