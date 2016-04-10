﻿using UnityEngine;
using System.Collections;
using System;
using RSG;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LiftLowerWaitRaise : Effect
{
    public float height = 1;
    public float speed = 1;
    public float pause = 1;

    public bool differentDownSpeed = false;
    public float downSpeed = 1;

    public float heightDelta = 0.05f;

    float currentHeight = 0;
    float waitingDuration;

    Action state;

    float actualHeight() {
        return height - heightDelta;
    }

    void Idle()
    {
    }

    public float TimeToGo() {
        if (state == Waiting) {
            return waitingDuration;
        }
        return float.PositiveInfinity;
    }

    void MovingDown()
    {
        var downSpeed = differentDownSpeed ? this.downSpeed : speed;
        float delta = downSpeed * TimeManager.StoppableFixedDeltaTime;
        bool ready = false;
        if (delta > actualHeight() - currentHeight) {
            ready = true;
            delta = actualHeight() - currentHeight;
        };
        currentHeight += delta;
        transform.Translate(Vector3.down * delta);
        if (ready)
        {
            state = Waiting;
            waitingDuration = pause;
        }
    }

    void Waiting()
    {
        waitingDuration -= TimeManager.StoppableFixedDeltaTime;
        if (waitingDuration < 0)
        {
            state = MovingUp;
        }
    }

    void MovingUp()
    {
        float delta = Math.Min(currentHeight, speed * TimeManager.StoppableFixedDeltaTime);
        currentHeight -= delta;
        transform.Translate(Vector3.up * delta);
        if (Mathf.Abs(currentHeight - 0) < Mathf.Epsilon)
        {
            state = Idle;
        }
    }

    public override IPromise Run()
    {
        if (!Ready()) {
            return Promise.Resolved();
        }
        state = MovingDown;
        return Promise.Resolved();
    }

    public override bool Ready() {
        return state != Waiting && state != MovingDown;
    }

    public override void Awake()
    {
        base.Awake();
        state = Idle;
    }

    public override void InitInternal() {
        new FloatTracker(v => waitingDuration = v, () => waitingDuration);
        new FloatTracker(v => currentHeight = v, () => currentHeight);
        new ValueTracker<Action>(v => state = v, () => state);
    }

    void FixedUpdate()
    {
        state();
    }

    [ContextMenu("Tracker to All Lifts")]
    void TrackersToAllLifts() {
        FindObjectsOfType<LiftLowerWaitRaise>().ToList().ForEach(lift => {
            if (lift.GetComponent<PositionTracker>() == null) {
                lift.gameObject.AddComponent<PositionTracker>();
            }
        });
    }
}