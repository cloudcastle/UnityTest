using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Cooldown
{
    public float duration;

    public float onCDfrom;

    public Cooldown(float duration) {
        this.duration = duration;
    }

    public bool Ready() {
        return TimeManager.GameTime > ReadySince();
    }

    public bool OnCooldown() {
        return !Ready();
    }

    public void StartCooldown() {
        onCDfrom = TimeManager.GameTime;
    }

    private float ReadySince() {
        return onCDfrom + duration;
    }
}