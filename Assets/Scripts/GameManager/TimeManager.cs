﻿using UnityEngine;
using System.Collections;
using RSG;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public static PromiseTimer promiseTimer = new PromiseTimer();

    public bool pauseOnStart;
    static bool paused;
    public static bool Paused {
        get {
            return paused;
        }
        set {
            if (value) {
                Pause();
            } else {
                Unpause();
            }
        }
    }
    public static bool timestopped = false;

    public float gameTime;

    public static float GameTime {
        get { return instance.gameTime; }
    }

    public static float loosedFixedDeltaTime;

    static void Pause()
    {
        paused = true;
        Time.timeScale = 0;
    }

    static void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
    }

    public static float GameFixedDeltaTime {
        get {
            if (timestopped) {
                return 0;
            } else {
                return Time.fixedDeltaTime;
            }
        }
    }

    static void SwitchPause()
    {
        if (Paused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    void FixedUpdate()
    {
        gameTime += Time.fixedDeltaTime;
        promiseTimer.Update(Time.fixedDeltaTime);
    }

    void Awake() {
        instance = this;
        Paused = pauseOnStart;
        timestopped = false;

        promiseTimer = new PromiseTimer();
    }

    void Start() {
        new FloatTracker((x) => gameTime = x, () => gameTime);
    }

    public static IPromise WaitFor(float time) {
        return promiseTimer.WaitFor(time);
    }
}