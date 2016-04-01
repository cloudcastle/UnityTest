using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
public class Walk : MonoBehaviour
{
    public float speed = 6;

    public float currentSpeed;

    void Awake() {
        GetComponent<Move>().additionalVelocities.Add(() => {
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            move = transform.TransformDirection(move);
            if (move.magnitude > 1) {
                move.Normalize();
            }
            move *= speed;
            currentSpeed = move.magnitude;
            move.y = 0;
            return move;
        });
    }
}