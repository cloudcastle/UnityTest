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
        Debug.Log("MovingDown");
        if (ready)
        {
            Debug.Log("Ready");
            state = Waiting;
            startWaitingMoment = Time.time;
        }
    }

    void Waiting()
    {
        Debug.Log("Time.time = " + Time.time);
        Debug.Log("End waiting time = " + (startWaitingMoment + pause));
        if (Time.time > startWaitingMoment + pause)
        {
            state = MovingUp;
        }
    }

    void MovingUp()
    {
        Debug.Log("Moving up");
        float delta = Math.Min(currentHeight, speed * Time.deltaTime);
        currentHeight -= delta;
        transform.Translate(Vector3.up * delta);
        if (Mathf.Abs(currentHeight - 0) < Mathf.Epsilon)
        {
            state = Idle;
            Debug.Log("Idle now");
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