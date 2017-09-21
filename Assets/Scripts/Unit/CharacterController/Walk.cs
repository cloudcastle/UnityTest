using UnityEngine;
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

    public Vector3 currentVelocity;

    /// <summary>
    /// Inertial velocity delta part left after each 1 second
    /// </summary>
    public float inertia = 0.01f;

    public override void Awake() {
        base.Awake();
        walkTransform = transform;
        GetComponent<Move>().additionalVelocities.Add(() => {
            var move = currentVelocity;

            currentSpeed = move.magnitude;
            if (zeroY) {
                move.y = 0;
            }
            return move;
        });
    }

    public void FixedUpdate() {
        var move = new Vector3(Controller.Move().x, 0, Controller.Move().y);
        move = walkTransform.TransformDirection(move);
        if (move.magnitude > 1) {
            move.Normalize();
        }
        move *= speed;
        currentVelocity = Vector3.Lerp(currentVelocity, move, 1-Mathf.Pow(inertia, TimeManager.FixedDeltaTime));
        if (Player.instance.current != this.unit) {
            currentVelocity = Vector3.zero;
        }
    }
}