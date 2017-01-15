using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class JumpRB : AbilityRB
{
    public float jumpSpeed = 8;
    public GroundCheckRB ground;

    bool jumpScheduled = false;

    float HalfTickGravityCorrection() {
        return Time.fixedDeltaTime * Physics.gravity.magnitude / 2; 
    }

    public override void InitInternal() {
        jumpSpeed += TimeManager.defaultFixedDeltaTime * Physics.gravity.magnitude / 2;
    }

    public override void Awake() {
        base.Awake();
        ground = GetComponent<GroundCheckRB>();
    }

    void Update() {
        if (enabled && unit.controller.Jump() && ground.grounded) {
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
        rb.velocity = transform.TransformVector(transform.InverseTransformVector(rb.velocity).Change(y: y));
        Debug.LogFormat("Jump, rb.velocity = {0}", rb.velocity);
        jumpScheduled = false;
    }
}