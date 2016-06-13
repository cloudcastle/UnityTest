using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Slowmo : Ability
{
    public float timeMultiplyer = 0.1f;
    public bool on = false;

    public override void InitInternal() {
        base.InitInternal();
        new BoolTracker(x => on = x, () => on);
    }

    void Update() {
        if (Player.instance.SlowmoSwitch()) {
            on = !on;
        }
    }
}