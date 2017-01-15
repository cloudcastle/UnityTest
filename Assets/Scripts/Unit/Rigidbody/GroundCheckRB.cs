using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(CapsuleCollider))]
public class GroundCheckRB : MonoBehaviour
{
    public bool grounded;

    public void FixedUpdate() {
        var preDistance = 0.01f;
        var postDistance = 0.01f;
        var border = 0.005f;
        var radius = 0.5f;
        var delta = 0.5f;
        var startPosition = Vector3.up * preDistance;
        var a = startPosition + Vector3.up * delta;
        var b = startPosition - Vector3.up * delta;
        var result = Physics.CapsuleCastAll(transform.TransformPoint(a), transform.TransformPoint(b), radius-border, transform.TransformDirection(Vector3.down), preDistance + postDistance);
        grounded = result.Count() > 1;
    }
}