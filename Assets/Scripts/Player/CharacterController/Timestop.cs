﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Timestop : MonoBehaviour
{
    void Update() {
        if (TimeManager.Paused) {
            return;
        }
        if (Input.GetButtonDown("Timestop")) {
            TimeManager.timestopped ^= true;
        }
    }
}