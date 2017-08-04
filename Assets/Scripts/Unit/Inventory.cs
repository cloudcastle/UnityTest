using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RSG;
using System.Linq;

public class Inventory : Ability
{
    public GameObject slotSample;
    public GameObject line;

    public Pool slotPool;
    public Transform inventoryArea;

    public List<Item> items;

    public Item selected;

    public event Action onChanged = () => { };
    public event Action onSkipAnimation = () => { };

    public Cooldown pickStun;

    public ItemListShallowTracker itemTracker;

    public override void InitInternal() {
        slotPool = Pool.CreatePool(slotSample);

        itemTracker = new ItemListShallowTracker(
            setList: (v) => items = v,
            getList: () => items
        );
        new ValueTracker<Item>(
            setValue: (v) => selected = v,
            getValue: () => selected
        );
        new ListShallowTracker<Poolable>(
            setList: (v) => slotPool.pool = v,
            getList: () => slotPool.pool
        );
        new ValueTracker<Action>(v => onChanged = v, () => onChanged);
        new ValueTracker<Action>(v => onSkipAnimation = v, () => onSkipAnimation);

        inventoryArea.transform.SetParent(Player.instance.inventoryAreas, worldPositionStays: false);
        inventoryArea.transform.localPosition = new Vector3(0, 100 * Unit.all.IndexOf(unit), 0);
    }

    public IPromise Pick(Item item, bool animate = true) {
        Init();
        if (pickStun.OnCooldown()) {
            Debug.Log("Pick on cooldown");
            return Promise.Resolved();
        }
        items.Add(item);
        selected = item;

        item.Picked(unit);

        if (DebugManager.debug) {
            Debug.Log(string.Format("Pick {0}", item));
        }
        GameObject slotObject = slotPool.Take();
        InventorySlot slot = slotObject.GetComponent<InventorySlot>();
        slot.Init(this, item);

        onChanged();
        if (animate == false) {
            SkipAnimation();
        }

        return slot.Ready();
    }

    public void Lose(Item item) {
        if (selected == item) {
            if (items.Count >= 2) {
                ChangeSelected(1);
            } else {
                selected = null; 
            }
        }
        items.Remove(item);
        if (DebugManager.debug) {
            Debug.Log(string.Format("Lost {0}", item));
        }

        item.inventorySlot.Free();
        item.Lost(unit);

        onChanged();
    }

    void DropAt(Item item, Vector3 position) {
        Lose(item);
        item.transform.position = position;
        item.GhostFor(unit);
        if (DebugManager.debug) {
            Debug.Log(String.Format("{0} thrown at {1}", item, position.ExtToString()));
        }
    }

    public void DropAll(Vector3 position, Func<Item, bool> predicate) {
        while (items.Count(predicate) > 0) {
            DropAt(items.First(predicate), position);
        }
    }

    void ChangeSelected(int delta = 1) {
        if (items.Count == 0) {
            return;
        }
        selected = items.CyclicNext(selected, delta);
        onChanged();
    }

    void SkipAnimation() {
        onSkipAnimation();
    }

    void Update() {
        if (Controller.NextItem()) {
            ChangeSelected(1);
        }
        if (Controller.PreviousItem()) {
            ChangeSelected(-1);
        }
    }
}