using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Move))]
[RequireComponent(typeof(UnitGeometryController))]
public class Jump : Ability
{
    public UnityEvent onJump;

    public float jumpSpeed = 8;

    bool jumpScheduled = false;

    UnitGeometryController characterController;

    float HalfTickGravityCorrection() {
        return Time.fixedDeltaTime * unit.gravity.gravity / 2; 
    }

    public override void Awake() {
        base.Awake();
        characterController = GetComponent<UnitGeometryController>();
    }

    public override void InitInternal() {
        jumpSpeed += TimeManager.defaultFixedDeltaTime * unit.gravity.gravity / 2;
    }

    void Update() {
        if (enabled && characterController.IsGrounded() && unit.controller.Jump()) {
            jumpScheduled = true;
        }
    }
    
    public void ChangeVelocity(Vector3 previousValue, Action<float> setX, Action<float> setY, Action<float> setZ) {
        if (jumpScheduled) {
            var y = jumpSpeed - HalfTickGravityCorrection();
            setY(y);
            jumpScheduled = false;
            onJump.Invoke();
        }
    }
}