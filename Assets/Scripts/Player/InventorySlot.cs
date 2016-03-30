using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TransformAnimator))]
public class InventorySlot : MonoBehaviour
{
    float animationDelay = 0.25f;

    public Inventory inventory;
    public Item item;

    TransformAnimator transformAnimator;

    void Awake() {
        transformAnimator = GetComponent<TransformAnimator>();
    }

    public void Init(Inventory inventory, Item item) {
        this.inventory = inventory;
        this.item = item;

        transform.SetParent(inventory.line.transform);
        item.transform.SetParent(transform, worldPositionStays: false);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        inventory.onChanged += OnInventoryChanged;

        transform.localPosition = TargetPosition();
        transform.localScale = TargetScale();
        transform.localRotation = Quaternion.identity;
        Debug.Log(string.Format("Slot inited; id = {1}, transform.position = {0}", transform.position, GetInstanceID()));
    }

    public Vector3 TargetPosition() {
        return new Vector3(inventory.items.IndexOf(item) * 2, 0, 0);
    }

    public Vector3 TargetScale() {
        return Vector3.one * (inventory.selected == item ? 1.5f : 1);
    }

    void OnInventoryChanged() {
        if (inventory.items.Contains(item)) {
            transformAnimator.Animate(new TimedValue<TransformState>(new TransformState(TargetPosition(), TargetScale()), Time.time + animationDelay));
        } else {
            Die();
        }
    }

    private void Die() {
        inventory.onChanged -= OnInventoryChanged;
        GetComponent<Poolable>().ReturnToPool();
    }
}