using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class StopRolling : MonoBehaviour
{
    Rigidbody rigidBody;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        var extinction = Mathf.Pow(2, TimeManager.StoppableFixedDeltaTime);
        if (Input.GetKey(KeyCode.X)) {
            rigidBody.velocity /= 1.001f;
            rigidBody.angularVelocity /= 1.001f;
        }
    }
}