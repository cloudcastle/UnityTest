﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
public class Walk : Ability
{
    public Transform walkTransform;
    public bool zeroY = false;

    public float speed = 6;

    public float currentSpeed;

    public override void Awake() {
        base.Awake();
        walkTransform = transform;
        GetComponent<Move>().additionalVelocities.Add(() => {
            var move = new Vector3(Controller.Move().x, 0, Controller.Move().y);
            move = walkTransform.TransformDirection(move);
            if (move.magnitude > 1) {
                move.Normalize();
            }
            move *= speed;
            currentSpeed = move.magnitude;
            if (zeroY) {
                move.y = 0;
            }
            return move;
        });
    }
}