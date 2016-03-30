using UnityEngine;

public class Item : Activatable
{
    float ghostTimeAfterThrow = 0.5f;

    public InventorySlot inventorySlot = null;
    public Player thrower;
    public float thrownAt;

    public override void Activate(Activator activator) {
        base.Activate(activator);
        activator.player.inventory.Pick(this);
    }

    public virtual void Pick(Player player) {
    }

    public virtual void Throw(Player thrower) {
        Physics.IgnoreCollision(thrower.GetComponent<Collider>(), GetComponent<Collider>());
        this.thrower = thrower;
        this.thrownAt = Time.time;
    }

    void FixedUpdate() {
        if (thrower != null) {
            if (Time.time > thrownAt + ghostTimeAfterThrow) {
                Physics.IgnoreCollision(thrower.GetComponent<Collider>(), GetComponent<Collider>(), false);
                thrower = null;
            }
        }
    }
}