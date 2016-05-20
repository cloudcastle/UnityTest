using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Slowmo : Ability
{
    public float timeMultiplyer = 0.1f;

    const float defaultFixedDeltaTime = 0.02f;

    public bool On() {
        return Controller.Slowmo() && !Controller.Rewind();
    }

    public override void Awake() {
        base.Awake();
        RefreshDeltaTime();
    }

    void RefreshDeltaTime() {
        TimeManager.loosedFixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    void Update() {
        if (TimeManager.Paused) {
            return;
        }
        if (Controller != Player.instance) {
            return;
        }
        if (Controller.Rewind()) {
            return;
        }
        Time.timeScale = (On() ? timeMultiplyer : 1);
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        //RefreshDeltaTime();
    }
}