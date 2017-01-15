using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(WalkRB))]
public class RunRB : Ability
{
    public float walkSpeed = 6;
    public float runSpeed = 18;

    WalkRB walk;

    public override void Awake() {
        base.Awake();
        walk = GetComponent<WalkRB>();
    }

    public bool Running() {
        return Controller.Run();
    }

    void Update() {
        walk.speed = Running() ? runSpeed : walkSpeed;
    }
}