using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Walk))]
public class Run : Ability
{
    public float walkSpeed = 6;
    public float runSpeed = 18;

    Walk walk;

    public override void Awake() {
        base.Awake();
        walk = GetComponent<Walk>();
    }

    public bool Running() {
        return Controller.Run();
    }

    void Update() {
        walk.speed = Running() ? runSpeed : walkSpeed;
    }
}