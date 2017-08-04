using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;

public class CooldownWatcher : MonoBehaviour
{
    public Cooldown cooldown;

    public float lastValue = -1;

    public FloatEvent valueChanged;

    public void Update() {
        var value = cooldown.OnCooldown() ? 1 : 0;
        if (value != lastValue) {
            lastValue = value;
            valueChanged.Invoke(value);
        }
    }

}