﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
public class Run : MonoBehaviour
{
    public float speed = 6;

    void Awake() {
        GetComponent<Move>().additionalVelocities.Add(() => {
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            move = transform.TransformDirection(move);
            move *= speed;
            move.y = 0;
            return move;
        });
    }
}