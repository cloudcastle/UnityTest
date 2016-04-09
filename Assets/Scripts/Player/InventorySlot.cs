using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TransformAnimator))]
public class InventorySlot : MonoBehaviour
{
    float animationDelay = 0.25f;

    public float selectedZoomMultiplier = 1.5f;
    public float baseZoom = 2f;

    public Inventory inventory;
    public Item item;

    TransformAnimator transformAnimator;

    public ItemTracker itemTracker;

    void Awake() {
        transformAnimator = GetComponent<TransformAnimator>();
    }

    void Start() {
        Debug.Log(string.Format("Start inventory slot {0}", GetInstanceID()));
        itemTracker = new ItemTracker(setValue: (v) => item = v, getValue: () => item);
        itemTracker.Init(null);
    }

    public void Init(Inventory inventory, Item item) {
        Debug.Log(string.Format("Init inventory slot {0}", GetInstanceID()));
        if (this.inventory != inventory) {
            inventory.onChanged += OnInventoryChanged;
        }
        this.inventory = inventory;
        this.item = item;
        item.inventorySlot = this;

        transform.SetParent(inventory.line.transform);
        item.transform.SetParent(transform, worldPositionStays: false);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        if (item.GetComponent<Rigidbody>() != null) {
            item.GetComponent<Rigidbody>().isKinematic = true;
        }

        transform.localPosition = TargetPosition();
        transform.localScale = TargetScale();
        transform.localRotation = Quaternion.identity;
    }

    public Vector3 TargetPosition() {
        return new Vector3(inventory.items.IndexOf(item) * 2, 0, 0);
    }

    public Vector3 TargetScale() {
        return Vector3.one * (inventory.selected == item ? selectedZoomMultiplier : 1) * baseZoom;
    }

    void OnInventoryChanged() {
        if (item != null) {
            if (inventory.items.Contains(item)) {
                transformAnimator.Animate(new TimedValue<TransformState>(new TransformState(TargetPosition(), TargetScale()), TimeManager.GameTime + animationDelay));
            } else {
                Free();
            }
        }
    }

    public void Free() {
        item.transform.SetParent(null, worldPositionStays: false);
        item.transform.position = inventory.player.transform.position;

        if (item.GetComponent<Rigidbody>() != null) {
            item.GetComponent<Rigidbody>().isKinematic = false;
        }

        item = null;

        GetComponent<Poolable>().ReturnToPool();
    }
}