using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GhostForm : Ability
{
    public bool on = false;

    public override void InitInternal() {
        base.InitInternal();
        new BoolTracker(x => on = x, () => on);
    }

    void Update() {
        if (!Cheats.on) {
            return;
        }
        if (Player.instance.GhostSwitch()) {
            on = !on;
            if (on) {
                unit.gravity.enabled = false;
                unit.fly.enabled = true;
                unit.gameObject.layer = LayerMask.NameToLayer("Ghost");
                unit.move.velocity = Vector3.zero;
                unit.walk.walkTransform = unit.head.transform;
                unit.jetpack.enabled = false;
            } else {
                unit.gravity.enabled = true;
                unit.fly.enabled = false;
                unit.gameObject.layer = LayerMask.NameToLayer("Unit");
                unit.walk.walkTransform = unit.transform;
                unit.jetpack.enabled = true;
            }
            Debug.LogFormat("Ghost Form {0}", on ? "on" : "off");
        }
    }
}