using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
[RequireComponent(typeof(UnitGeometryController))]
public class Ground : MonoBehaviour
{
    const float groundedVerticalVelocity = -1f;

    UnitGeometryController characterController;

    void Awake() {
        characterController = GetComponent<UnitGeometryController>();
    }

    void Update() {
    }

    public void ChangeVelocity(Vector3 previousValue, Action<float> setX, Action<float> setY, Action<float> setZ) {
        if (enabled) {
            if (characterController.IsGrounded()) {
                if (previousValue.y < 0) {
                    setY(groundedVerticalVelocity);
                }
            }
        }
    }
}