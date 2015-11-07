using UnityEngine;
using System.Collections;
using System;

public class LiftLowerWaitRaise : Effect
{
    public float height = 1;
    public float speed = 1;
    public float pause = 1;

    float currentHeight = 0;
    float startWaitingMoment;

    Action state;

    void Idle()
    {
    }

    void MovingDown()
    {
        float delta = Math.Min(height - currentHeight, speed * Time.deltaTime);
        currentHeight += delta;
        transform.Translate(Vector3.down * delta);
        if (Mathf.Abs(currentHeight - height) < Mathf.Epsilon)
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