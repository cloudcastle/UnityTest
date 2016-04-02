using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    public Vector3 velocity = Vector3.zero;
    public Vector3Tracker velocityTracker;

    public Vector3 readonlyVelocity;
    public bool readonlyGrounded;

    public List<Func<Vector3>> additionalVelocities = new List<Func<Vector3>>();

    CharacterController controller;
    ChangeScale changeScale;

    Ground ground;
    Jump jump;
    AntigravityEffect antigravityEffect;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        changeScale = GetComponent<ChangeScale>();

        ground = GetComponent<Ground>();
        jump = GetComponent<Jump>();
        antigravityEffect = GetComponent<AntigravityEffect>();
    }

    void Start() {
        velocityTracker = new Vector3Tracker(
            (v) => velocity = v,
            () => velocity
        );
    }

    Vector3 TotalVelocity() {
        return additionalVelocities.Aggregate(velocity, (acc, av) => acc + av());
    }

    void SetVelocityX(float value) {
        velocity.x = value;
    }
    void SetVelocityY(float value) {
        velocity.y = value;
    }
    void SetVelocityZ(float value) {
        velocity.z = value;
    }
    void ChangeVelocity(Action<Vector3, Action<float>, Action<float>, Action<float>> changer) {
        changer(velocity, SetVelocityX, SetVelocityY, SetVelocityZ);
    }

    void FixedUpdate() {
        readonlyVelocity = velocity;
        readonlyGrounded = controller.isGrounded;
        if (TimeManager.paused)
        {
            return;
        }

        if (ground != null) {
            ChangeVelocity(ground.ChangeVelocity);
        }
        if (jump != null) {
            ChangeVelocity(jump.ChangeVelocity);
        }
        if (antigravityEffect != null) {
            ChangeVelocity(antigravityEffect.ChangeVelocity);
        }

        controller.Move(currentScale() * TotalVelocity() * Time.fixedDeltaTime);
    }

    float currentScale() {
        return (changeScale == null) ? 1 : changeScale.currentScale;
    }

    public void Accelerate(Vector3 value) {
        velocity += value;
    }
}