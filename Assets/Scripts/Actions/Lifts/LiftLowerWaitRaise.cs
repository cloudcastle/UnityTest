using UnityEngine;
using System.Collections;
using System;

public class LiftLowerWaitRaise : Effect
{
    public float height = 1;
    public float speed = 1;
    public float pause = 1;

    public float heightDelta = 0.05f;

    float currentHeight = 0;
    float startWaitingMoment;

    Action state;

    float actualHeight() {
        return height - heightDelta;
    }

    void Idle()
    {
    }

    void MovingDown()
    {
        float delta = speed * Time.deltaTime;
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
            startWaitingMoment = Time.time;
        }
    }

    void Waiting()
    {
        if (Time.time > startWaitingMoment + pause)
        {
            state = MovingUp;
        }
    }

    void MovingUp()
    {
        float delta = Math.Min(currentHeight, speed * Time.deltaTime);
        currentHeight -= delta;
        transform.Translate(Vector3.up * delta);
        if (Mathf.Abs(currentHeight - 0) < Mathf.Epsilon)
        {
            state = Idle;
        }
    }

    public override void Run()
    {
        state = MovingDown;
    }

    void Awake()
    {
        state = Idle;
    }

    void Update()
    {
        state();
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Run();
        }
    }
}