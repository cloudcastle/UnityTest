using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
public class Fly : Ability
{
    public float speed = 6;

    public float currentSpeed;

    public override void Awake() {
        base.Awake();
        GetComponent<Move>().additionalVelocities.Add(() => {
            if (!Cheats.on) {
                return Vector3.zero;
            }
            var move = new Vector3(0, Controller.Fly(), 0);
            move = transform.TransformDirection(move);
            if (move.magnitude > 1) {
                move.Normalize();
            }
            move *= speed;
            currentSpeed = move.magnitude;
            return move;
        });
    }
}