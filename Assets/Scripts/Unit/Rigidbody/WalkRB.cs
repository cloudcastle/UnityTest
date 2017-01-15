﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class WalkRB : AbilityRB
{
    public float speed = 6;

    public float currentSpeed;

    public Vector3 Move() {
        var move = new Vector3(Controller.Move().x, 0, Controller.Move().y);
        if (move.magnitude > 1) {
            move.Normalize();
        }
        move *= speed;
        currentSpeed = move.magnitude;
        move.y = 0;
        return move;
    }

    void FixedUpdate() {
        var move = Move();
        var localVelocity = transform.InverseTransformVector(rb.velocity);
        rb.velocity = transform.TransformVector(localVelocity.Change(x: move.x, z: move.z));
    }
}