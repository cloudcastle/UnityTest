using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class RigidBodyTracker : MonoBehaviour
{
    public new Rigidbody rigidbody;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    public Vector3Tracker velocityTracker;
    public Vector3Tracker angularVelocityTracker;
    public BoolTracker isKinematicTracker;

    void Start() {
        velocityTracker = new Vector3Tracker((v) => rigidbody.velocity = v, () => rigidbody.velocity);
        angularVelocityTracker = new Vector3Tracker((v) => rigidbody.angularVelocity = v, () => rigidbody.angularVelocity);
        isKinematicTracker = new BoolTracker((v) => rigidbody.isKinematic = v, () => rigidbody.isKinematic);
    }
}