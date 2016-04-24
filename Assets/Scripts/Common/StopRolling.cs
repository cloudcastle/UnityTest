using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class StopRolling : MonoBehaviour
{
    Rigidbody rigidBody;
    public float readonlyAngularDrag;
    public float minAngularDrag = 1;
    public float extinctionSpeed = 0.5f;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start() {
        new FloatTracker(v => rigidBody.angularDrag = v, () => rigidBody.angularDrag);
    }

    float Extinction() {
        return Mathf.Pow(2, TimeManager.StoppableFixedDeltaTime * extinctionSpeed);
    }

    void FixedUpdate() {
        if (rigidBody.angularDrag > minAngularDrag) {
            rigidBody.angularDrag /= Extinction();
        }
        readonlyAngularDrag = rigidBody.angularDrag;
    }

    void OnCollisionStay(Collision collision) {
        ContactPoint contact = collision.contacts.First();
        if (contact.normal.y > 0.99 && contact.otherCollider is MeshCollider) {
            rigidBody.angularDrag *= Extinction();
            rigidBody.angularDrag *= Extinction();
        } else if (contact.otherCollider.GetComponent<Rigidbody>() != null) {
            rigidBody.angularDrag = minAngularDrag;
        }
    }
}