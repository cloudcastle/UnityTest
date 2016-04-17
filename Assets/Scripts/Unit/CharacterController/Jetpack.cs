using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
[RequireComponent(typeof(CharacterController))]
public class Jetpack : Ability
{
    public float acceleration = 22;
    public float ignitionTime = 1;
    public float ignitionTimePassed = 0;

    CharacterController characterController;
    Move move;

    public override void Awake() {
        base.Awake();
        characterController = GetComponent<CharacterController>();
        move = GetComponent<Move>();
    }

    void FixedUpdate() {
        if (unit.controller.Jetpack()) {
            ignitionTimePassed += Time.fixedDeltaTime;
            if (ignitionTimePassed > ignitionTime) {
                move.Accelerate(acceleration * Vector3.up * Time.fixedDeltaTime);
            }
        }
        if (characterController.isGrounded) {
            ignitionTimePassed = 0;
        }
    }
}