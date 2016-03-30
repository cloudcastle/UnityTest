using UnityEngine;

public class Item : Activatable
{
    public override void Activate(Activator activator) {
        base.Activate(activator);
        activator.player.inventory.Pick(this);
    }
}