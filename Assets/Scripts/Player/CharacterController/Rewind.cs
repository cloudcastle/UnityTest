using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Rewind : MonoBehaviour
{
    public float timeMultiplyer = 8;

    const float defaultFixedDeltaTime = 0.02f;

    public bool On() {
        return Input.GetButton("Rewind");
    }

    void Awake() {
        RefreshDeltaTime();
    }

    void RefreshDeltaTime() {
        TimeManager.loosedFixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    void Update() {
        if (TimeManager.Paused) {
            return;
        }
        Time.timeScale = (On() ? timeMultiplyer : 1);
        //Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        RefreshDeltaTime();
    }
}