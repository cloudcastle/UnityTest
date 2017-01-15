using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(CapsuleCollider))]
public class GroundRB : AbilityRB
{
    public bool grounded;

    public override void Awake() {
        base.Awake();
    }

    public void FixedUpdate() {
        var preDistance = 0.01f;
        var postDistance = 0.01f;
        var radius = 0.5f;
        var delta = 0.5f;
        var startPosition = Vector3.up * preDistance;
        var a = startPosition + Vector3.up * delta;
        var b = startPosition - Vector3.up * delta;
        grounded = Physics.CapsuleCast(transform.TransformPoint(a), transform.TransformPoint(b), radius, transform.TransformDirection(Vector3.down), preDistance + postDistance);
    }
}