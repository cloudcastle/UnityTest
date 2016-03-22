﻿using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public bool pauseOnStart;
    public static bool paused = true;
    public static bool timestopped = false;

    public float time;

    void Pause()
    {
        paused = true;
        Time.timeScale = 0;
    }

    void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
    }

    public static float GameFixedDeltaTime() {
        if (timestopped) {
            return 0;
        } else {
            return Time.fixedDeltaTime;
        }
    }

    void SwitchPause()
    {
        if (paused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel/Menu"))
        {
            SwitchPause();
        }
        time = Time.time;
    }

    void Awake() {
        paused = pauseOnStart;
        timestopped = false;
    }
}