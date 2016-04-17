using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
[RequireComponent(typeof(CharacterController))]
public class Jump : Ability
{
    public float jumpSpeed = 8;

    bool jumpScheduled = false;

    CharacterController characterController;

    public override void Awake() {
        base.Awake();
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        if (characterController.isGrounded && unit.controller.Jump()) {
            jumpScheduled = true;
        }
    }
    
    public void ChangeVelocity(Vector3 previousValue, Action<float> setX, Action<float> setY, Action<float> setZ) {
        if (jumpScheduled) {
            setY(jumpSpeed);
            jumpScheduled = false;
        }
    }
}