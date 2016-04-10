using UnityEngine;
using System;

public class Item : Activatable
{
    const float ghostTimeAfterThrow = 0.5f;

    public InventorySlot inventorySlot = null;

    public event Action<Player> onPick = (p) => { };
    public event Action<Player> onLose = (p) => { };

    public override void Start() {
        base.Start();
        new ValueTracker<InventorySlot>(v => inventorySlot = v, () => inventorySlot);
    }

    public override void Activate(Activator activator) {
        base.Activate(activator);
        activator.player.inventory.Pick(this);
    }

    public void Picked(Player player) {
        onPick(player);
    }

    public void Lost(Player player) {
        onLose(player);
    }

    public void GhostFor(Player player) {
        Debug.Log(String.Format("Item {0} ghost for {1}", this, player));
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        TimeManager.WaitFor(ghostTimeAfterThrow).Then(() => {
            Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), false);
            Debug.Log(String.Format("Item {0} unghost for {1}", this, player));
        });
    }
}