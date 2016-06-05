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

    float HalfTickGravityCorrection() {
        return Time.fixedDeltaTime * unit.gravity.gravity / 2; 
    }

    public override void Awake() {
        base.Awake();
        characterController = GetComponent<CharacterController>();
    }

    void Start() {
        jumpSpeed += HalfTickGravityCorrection(); 
    }

    void Update() {
        if (enabled && characterController.isGrounded && unit.controller.Jump()) {
            jumpScheduled = true;
        }
    }
    
    public void ChangeVelocity(Vector3 previousValue, Action<float> setX, Action<float> setY, Action<float> setZ) {
        if (jumpScheduled) {
            setY(jumpSpeed - HalfTickGravityCorrection());
            jumpScheduled = false;
        }
    }
}