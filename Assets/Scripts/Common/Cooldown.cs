using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Cooldown : MonoBehaviour
{
    public float duration;
    public FloatTracker tracker;
    public Func<float> getTime = () => TimeManager.GameTime;

    public float onCDfrom = float.NegativeInfinity;

    public void Awake() {
        tracker = new FloatTracker((v) => onCDfrom = v, () => onCDfrom);
        tracker.Init(onCDfrom);
    }

    public bool Ready() {
        return getTime() > ReadySince();
    }

    public bool OnCooldown() {
        return !Ready();
    }

    public void StartCooldown() {
        onCDfrom = getTime();
    }

    private float ReadySince() {
        return onCDfrom + duration;
    }
}