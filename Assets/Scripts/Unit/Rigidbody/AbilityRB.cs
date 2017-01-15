using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody))]
public class AbilityRB : Ability
{
    public Rigidbody rb;

    public override void Awake() {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }
}