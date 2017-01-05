using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour {
    public Vector3 centerOfMass;
    public Vector3 inertiaTensor;

    void Start() {
        var rb = GetComponentInParent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        rb.inertiaTensor = inertiaTensor;
    }
}
