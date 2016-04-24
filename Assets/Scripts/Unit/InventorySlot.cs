using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RSG;
using System;

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

    bool started = false;
    event Action onStart = () => { };

    void Awake() {
        transformAnimator = GetComponent<TransformAnimator>();
    }

    void Start() {
        Debug.Log(string.Format("Inventory Slot start"));
        itemTracker = new ItemTracker(setValue: (v) => item = v, getValue: () => item);
        new ValueTracker<Inventory>(setValue: (v) => inventory = v, getValue: () => inventory);
        itemTracker.Init(null);
        onStart();
        started = true;
    }

    public IPromise Ready() {
        if (started) {
            return Promise.Resolved();
        }
        var result = new Promise();
        Debug.Log("new promise created");
        onStart += result.Resolve;
        return result.WithName(string.Format("Inventory slot ready: {0}", name));
    }

    void Subscribe(Inventory inventory) {
        inventory.onChanged += OnInventoryChanged;
        inventory.onSkipAnimation += OnSkipAnimation;
    }

    void Unsubscribe(Inventory inventory) {
        this.inventory.onChanged -= OnInventoryChanged;
        inventory.onSkipAnimation -= OnSkipAnimation;
    }

    public void Init(Inventory inventory, Item item) {
        if (this.inventory != inventory) {
            if (this.inventory != null) {
                Unsubscribe(this.inventory);
            }
            Subscribe(inventory);
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
        if (inventory.items.Contains(item)) {
            transformAnimator.Animate(new TimedValue<TransformState>(new TransformState(TargetPosition(), scale: TargetScale()), TimeManager.GameTime + animationDelay));
        } else {
            Free();
        }
    }

    void OnSkipAnimation() {
        transformAnimator.SkipAnimation();
    }

    public void Free() {
        if (item == null) {
            throw new Exception("Item is null in InventorySlot.Free!");
        }
        item.transform.SetParent(null, worldPositionStays: false);

        if (item.GetComponent<Rigidbody>() != null) {
            item.GetComponent<Rigidbody>().isKinematic = false;
        }

        Unsubscribe(inventory);
        item = null;
        inventory = null;

        GetComponent<Poolable>().ReturnToPool();
    }

    void Update() {
        name = item == null ? "Empty slot" : String.Format("Slot ({0})", item.name);
    }
}