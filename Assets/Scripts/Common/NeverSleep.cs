using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class NeverSleep : MonoBehaviour
{
    Rigidbody rigidBody;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.sleepThreshold = 0;
    }

    void FixedUpdate() {
        if (rigidBody.IsSleeping()) {
            Debug.Log("Sleeping");
        }
    }
}