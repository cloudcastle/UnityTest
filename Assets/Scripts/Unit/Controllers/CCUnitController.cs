using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(CharacterController))]
public class CCUnitController : UnitGeometryController
{
    CharacterController characterController;

    public void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    public override bool IsGrounded() {
        return characterController.isGrounded;
    }

    public override void SetVelocity(Vector3 v) {
        characterController.Move(transform.TransformVector(v) * Time.fixedDeltaTime);
    }
}