using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
public class CCGravity : MonoBehaviour
{
    public float gravity = 20;

    UnitGeometryController characterController;
    Move move;

    void Awake() {
        characterController = GetComponent<UnitGeometryController>();
        move = GetComponent<Move>();
    }

    void FixedUpdate() {
        if (TimeManager.Paused) {
            return;
        }
        if (!characterController.IsGrounded()) {
            move.Accelerate(Time.fixedDeltaTime * gravity * Vector3.down);
        }
    }
}