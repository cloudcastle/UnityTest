using UnityEngine;

public class Item : Activatable
{
    const float ghostTimeAfterThrow = 0.5f;

    public InventorySlot inventorySlot = null;
    public Player thrower;
    public float thrownAt;

    void Start() {
        new ValueTracker<InventorySlot>(v => inventorySlot = v, () => inventorySlot);
        new ValueTracker<Player>(v => thrower = v, () => thrower);
        new FloatTracker(v => thrownAt = v, () => thrownAt);
    }

    public override void Activate(Activator activator) {
        base.Activate(activator);
        activator.player.inventory.Pick(this);
    }

    public virtual void Pick(Player player) {
    }

    public virtual void Throw(Player thrower) {
        Physics.IgnoreCollision(thrower.GetComponent<Collider>(), GetComponent<Collider>());
        this.thrower = thrower;
        this.thrownAt = TimeManager.GameTime;
    }

    void FixedUpdate() {
        if (thrower != null) {
            if (TimeManager.GameTime > thrownAt + ghostTimeAfterThrow) {
                Physics.IgnoreCollision(thrower.GetComponent<Collider>(), GetComponent<Collider>(), false);
                thrower = null;
            }
        }
    }
}