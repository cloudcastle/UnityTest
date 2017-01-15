using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class UnitGeometryController : MonoBehaviour
{
    public abstract bool IsGrounded();
    public abstract void SetVelocity(Vector3 v);
}