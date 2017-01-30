using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(CapsuleCollider))]
public class GroundCheckRB : AbilityRB
{
    public bool grounded;
    public GameObject ground;

    public void FixedUpdate() {
        var preDistance = 0.01f;
        var postDistance = 0.05f;
        var border = 0.005f;
        var radius = 0.5f;
        var delta = 0.5f;
        var startPosition = Vector3.up * preDistance;
        var a = startPosition + Vector3.up * delta;
        var b = startPosition - Vector3.up * delta;
        var result = Physics.CapsuleCastAll(transform.TransformPoint(a), transform.TransformPoint(b), radius - border, transform.TransformDirection(Vector3.down), preDistance + postDistance, ~LayerMask.GetMask("Ghost"));
        var filtered = result.Where(h => {
            var door = h.collider.GetComponent<Door>();
            return door == null || !door.Opened(unit);
        }).ToList();
        grounded = filtered.Count() > 1;
        if (grounded) {
            ground = filtered[0].collider.gameObject;
        }
    }
}