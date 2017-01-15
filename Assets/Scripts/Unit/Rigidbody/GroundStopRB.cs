using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(GroundRB))]
public class GroundStopRB : AbilityRB
{
    public bool grounded;

    public override void Awake() {
        base.Awake();
    }

    public void FixedUpdate() {
    }
}