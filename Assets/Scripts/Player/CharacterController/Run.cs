using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Walk))]
public class Run : MonoBehaviour
{
    public float walkSpeed = 6;
    public float runSpeed = 18;

    Walk walk;

    void Awake() {
        walk = GetComponent<Walk>();
    }

    public bool Running() {
        return Input.GetButton("Run");
    }

    void Update() {
        walk.speed = Running() ? runSpeed : walkSpeed;
    }
}