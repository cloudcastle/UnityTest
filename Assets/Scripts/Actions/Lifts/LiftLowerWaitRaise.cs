using UnityEngine;
using System.Collections;
using System;
using RSG;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
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

    Action State {
        set {
            state = value;
            onSpeedChanged.Invoke(value == Idle || value == Waiting ? 0 : 1);
        }
        get {
            return state;
        }
    }

    float actualHeight() {
        return height - heightDelta;
    }

    public FloatEvent onSpeedChanged;

    void Idle()
    {
    }

    public float TimeToGo() {
        if (State == Waiting) {
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
            State = Waiting;
            waitingDuration = pause;
        }
    }

    void Waiting()
    {
        waitingDuration -= TimeManager.StoppableFixedDeltaTime;
        if (waitingDuration < 0)
        {
            State = MovingUp;
            FindObjectsOfType<Rigidbody>().ToList().ForEach(rb => rb.WakeUp());
        }
    }

    void MovingUp()
    {
        float delta = Math.Min(currentHeight, speed * TimeManager.StoppableFixedDeltaTime);
        currentHeight -= delta;
        transform.Translate(Vector3.up * delta);
        if (Mathf.Abs(currentHeight - 0) < Mathf.Epsilon)
        {
            State = Idle;
        }
    }

    public override IPromise Run()
    {
        if (!Ready()) {
            return Promise.Resolved();
        }
        State = MovingDown;
        return Promise.Resolved();
    }

    bool Ready() {
        return State != Waiting && State != MovingDown;
    }

    public override ActivatableStatus Status() {
        return Ready() ? ActivatableStatus.Activatable : ActivatableStatus.Activated;
    }

    public override void Awake()
    {
        base.Awake();
        State = Idle;
    }

    public override void InitInternal() {
        new FloatTracker(v => waitingDuration = v, () => waitingDuration);
        new FloatTracker(v => currentHeight = v, () => currentHeight);
        new ValueTracker<Action>(v => State = v, () => State);
    }

    void FixedUpdate()
    {
        state();
    }

#if UNITY_EDITOR
    [ContextMenu("Tracker to All Lifts")]
    void TrackersToAllLifts() {
        FindObjectsOfType<LiftLowerWaitRaise>().ToList().ForEach(lift => {
            if (lift.GetComponent<PositionTracker>() == null) {
                lift.gameObject.AddComponent<PositionTracker>();
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        });
    }
#endif
}