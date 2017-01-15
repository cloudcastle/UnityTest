using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundCheckRB))]
public class RBUnitController : UnitGeometryController
{
    Rigidbody rigidBody;
    GroundCheckRB groundCheckRB;

    public void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        groundCheckRB = GetComponent<GroundCheckRB>();
    }

    public override bool IsGrounded() {
        return groundCheckRB.grounded;
    }

    public override void SetVelocity(Vector3 v) {
        rigidBody.velocity = transform.TransformVector(v);
    }
}