using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Move))]
public class AntigravityEffect : MonoBehaviour
{
    const float duration = 0.05f;

    public float speed = 0;

    Move move;

    [HideInInspector]
    public float antigravityUntil = float.NegativeInfinity;

    bool Expired() {
        return antigravityUntil < Time.time;
    }

    public void Affect(float speed) {
        if (this.speed <= speed || Expired()) {
            this.speed = speed;
            antigravityUntil = Time.time + duration;
        }
    }

    void Awake() {
        move = GetComponent<Move>();
        move.additionalVelocities.Add(() => {
            if (Expired()) {
                speed = 0;
            }
            return speed * Vector3.up;
        });
    }

    public void ChangeVelocity(Vector3 velocity) {

    }

    public void ChangeVelocity(Vector3 previousValue, Action<float> setX, Action<float> setY, Action<float> setZ) {
        if (!Expired()) {
            setY(0);
        }
    }
}