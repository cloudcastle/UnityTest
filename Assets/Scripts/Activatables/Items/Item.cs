using UnityEngine;
using System;

public class Item : Activatable
{
    const float ghostTimeAfterThrow = 0.5f;

    public InventorySlot inventorySlot = null;

    public event Action<Unit> onPick = (p) => { };
    public event Action<Unit> onLose = (p) => { };

    public override void Start() {
        base.Start();
        new ValueTracker<InventorySlot>(v => inventorySlot = v, () => inventorySlot);
    }

    public override void Activate(Activator activator) {
        base.Activate(activator);
        activator.unit.inventory.Pick(this);
    }

    public override ActivatableStatus Status() {
        if (Player.instance.current.inventory.pickStun.OnCooldown()) {
            return ActivatableStatus.Inactive;
        }
        return ActivatableStatus.Activatable;
    }

    public void Picked(Unit player) {
        onPick(player);
    }

    public void Lost(Unit player) {
        onLose(player);
    }

    public void GhostFor(Unit player) {
        Debug.Log(String.Format("Item {0} ghost for {1}", this, player));
        Extensions.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        TimeManager.WaitFor(ghostTimeAfterThrow).Then(() => {
            Extensions.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), false);
            Debug.Log(String.Format("Item {0} unghost for {1}", this, player));
        });
    }
}