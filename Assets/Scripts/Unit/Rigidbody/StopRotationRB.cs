using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class StopRotationRB : AbilityRB
{
    void FixedUpdate() {
        rb.angularVelocity = Vector3.zero;
    }
}