using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class MovingSurface : MonoBehaviour {

    public Vector3Event onVelocityChange;

    public Vector3 currentVelocity;
    public Vector3 currentAngularVelocity;
}