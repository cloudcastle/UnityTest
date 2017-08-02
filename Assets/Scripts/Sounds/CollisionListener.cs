using UnityEngine;
using System.Collections;
using RSG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;
using UnityEngine.Events;

public class CollisionListener : MonoBehaviour
{
    public Vector3Event onCollide;

    public void OnCollisionEnter(Collision c) {
        onCollide.Invoke(c.relativeVelocity);
    }
}