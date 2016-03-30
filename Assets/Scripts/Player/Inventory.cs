using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour
{
    public GameObject slotSample;
    public GameObject line;
    public Player player;

    Pool slotPool;

    public List<Item> items;

    public Item selected;

    public event Action onChanged = () => { };

    void Awake() {
        slotPool = new Pool(slotSample);
    }

    public void Pick(Item item) {
        items.Insert(0, item);
        if (selected == null) {
            selected = item;
        }

        Debug.Log(string.Format("Pick {0}", item));
        GameObject slotObject = slotPool.Take();
        InventorySlot slot = slotObject.GetComponent<InventorySlot>();
        slot.Init(this, item);

        onChanged();
    }

    public void Throw(Item item) {
        if (selected == item) {
            if (items.Count >= 2) {
                ChangeSelected(1);
            } else {
                selected = null; 
            }
        }
        items.Remove(item);
        Debug.Log(string.Format("Throw {0}", item));

        item.inventorySlot.Free();
        item.Throw(player);

        onChanged();
    }

    public void DropAll() {
        while (items.Count > 0) {
            Throw(selected);
        }
    }

    void ChangeSelected(int delta = 1) {
        if (items.Count == 0) {
            return;
        }
        selected = items.CyclicNext(selected, delta);
        onChanged();
    }

    void Update() {
        if (Input.GetButtonDown("Next Item")) {
            ChangeSelected(1);
        }
        if (Input.GetButtonDown("Previous Item")) {
            ChangeSelected(-1);
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) {
            ChangeSelected(-1);
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) {
            ChangeSelected(1);
        }
    }
}