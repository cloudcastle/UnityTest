using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class JumpRB : AbilityRB
{
    public float jumpSpeed = 8;

    bool jumpScheduled = false;

    float HalfTickGravityCorrection() {
        return Time.fixedDeltaTime * Physics.gravity.magnitude / 2; 
    }

    public override void InitInternal() {
        jumpSpeed += TimeManager.defaultFixedDeltaTime * Physics.gravity.magnitude / 2;
    }

    void Update() {
        if (enabled && unit.controller.Jump()) {
            jumpScheduled = true;
        }
    }

    void FixedUpdate() {
        if (jumpScheduled) {
            Jump();
        }
    }

    private void Jump() {
        var y = jumpSpeed - HalfTickGravityCorrection();
        rb.velocity = rb.velocity.Change(y: y);
        Debug.LogFormat("Jump, rb.velocity = {0}", rb.velocity);
        jumpScheduled = false;
    }
}