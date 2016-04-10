using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class Cooldown
{
    public float duration;
    public FloatTracker tracker;

    public float onCDfrom = float.NegativeInfinity;

    public Cooldown(float duration) {
        this.duration = duration;
        tracker = new FloatTracker((v) => onCDfrom = v, () => onCDfrom);
        tracker.Init(onCDfrom);
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