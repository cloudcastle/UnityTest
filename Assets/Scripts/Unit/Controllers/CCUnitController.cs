using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(CharacterController))]
public class CCUnitController : UnitGeometryController
{
    CharacterController characterController;

    bool isGroundedValid = true;

    public void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    public void Start() {
        TimeManager.instance.onUndo += () => {
            isGroundedValid = false;
        };
    }

    public override bool IsGrounded() {
        return characterController.isGrounded && isGroundedValid;
    }

    public override void SetVelocity(Vector3 v) {
        isGroundedValid = true;
        characterController.Move(v * Time.fixedDeltaTime);
    }
}