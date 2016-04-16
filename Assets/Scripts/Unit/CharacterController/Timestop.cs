using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Timestop : Ability
{
    void Update() {
        if (TimeManager.Paused) {
            return;
        }
        if (unit.controller.ToggleTimestop()) {
            TimeManager.timestopped ^= true;
        }
    }
}