using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GravityRB : AbilityRB
{
    public Vector3 gravity = Vector3.down * 20;

    public override void Awake() {
        base.Awake();
        rb.useGravity = false;
    }

    void FixedUpdate() {
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}