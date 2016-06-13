using UnityEngine;
using System;

public class Item : Activatable
{
    const float ghostTimeAfterThrow = 0.1f;

    public InventorySlot inventorySlot = null;

    public event Action<Unit> onPick = (p) => { };
    public event Action<Unit> onLose = (p) => { };

    bool ghostForm = false;
    Unit thrower = null;

    public override void Start() {
        base.Start();
        new ValueTracker<InventorySlot>(v => inventorySlot = v, () => inventorySlot);
        new ValueTracker<bool>(v => {
            ghostForm = v;
            SetSemitransparent(ghostForm);
            if (thrower != null) {
                Extensions.IgnoreCollision(thrower, this, v);
            }
        }, () => ghostForm);
        new ValueTracker<Unit>(v => thrower = v, () => thrower);

        bool ghost = GetComponent<NotCollidePlayer>() != null;
        if (ghost) {
            GhostFor(FindObjectOfType<Unit>());
        } else {
            SetSemitransparent(false);
        }
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
        SetSemitransparent(false);
    }

    public void Lost(Unit player) {
        onLose(player);
    }

    public virtual void SetSemitransparent(bool on = true) {
    }

    bool OverlappingWithThrower() {
        SpaceScanner.overlapCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            GetComponent<SphereCollider>().radius * transform.lossyScale.x,
            SpaceScanner.overlapResults,
            LayerMask.GetMask("Unit")
        );
        for (int i = 0; i < SpaceScanner.overlapCount; i++) {
            var unit = SpaceScanner.overlapResults[i].gameObject.GetComponent<Unit>();
            if (unit == thrower) {
                return true;
            }
        }
        return false;
    }

    public void FixedUpdate() {
        if (ghostForm) {
            if (!OverlappingWithThrower()) {
                GhostFormOff();
            }
        }
    }

    void GhostFormOff() {
        if (GetComponent<NotCollidePlayer>() != null) {
            return;
        }
        Extensions.IgnoreCollision(thrower, this, false);
        SetSemitransparent(false);
        ghostForm = false;
        if (DebugManager.debug) {
            Debug.Log(String.Format("Item {0} unghost for {1}", this, thrower));
        }
    }

    public void GhostFor(Unit player) {
        if (DebugManager.debug) {
            Debug.Log(String.Format("Item {0} ghost for {1}", this, player));
        }
        Extensions.IgnoreCollision(player, this);
        SetSemitransparent();
        ghostForm = true;
        thrower = player;
    }
}