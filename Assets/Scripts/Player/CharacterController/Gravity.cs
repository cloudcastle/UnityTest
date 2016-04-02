using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
public class Gravity : MonoBehaviour
{
    public float gravity = 20;

    CharacterController characterController;
    Move move;

    void Awake() {
        characterController = GetComponent<CharacterController>();
        move = GetComponent<Move>();
    }

    void FixedUpdate() {
        if (TimeManager.paused) {
            return;
        }
        if (!characterController.isGrounded) {
            move.Accelerate(Time.fixedDeltaTime * gravity * Vector3.down);
        }
    }
}