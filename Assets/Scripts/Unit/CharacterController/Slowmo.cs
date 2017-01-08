using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Slowmo : Ability, ISlowmo
{
    public float timeMultiplyer = 0.1f;
    public bool on = false;

    public override void InitInternal() {
        base.InitInternal();
        new BoolTracker(x => on = x, () => on);
        TimeManager.instance.slowmos.Add(this);
    }

    void Update() {
        if (Player.instance.SlowmoSwitch()) {
            on = !on;
        }
    }

    public float SlowmoMultiplier() {
        return on && Player.instance.current == Controller ? timeMultiplyer : 1;
    }
}