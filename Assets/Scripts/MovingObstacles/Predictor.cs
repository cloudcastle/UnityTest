﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Predictor : MovingSurface {
    public void Predict(Vector3 position, Vector3 rotation) {
        if (TimeManager.StoppableFixedDeltaTime < 1e-4) {
            currentVelocity = Vector3.zero;
            currentAngularVelocity = Vector3.zero;
        }
        currentVelocity = (position - transform.position) / TimeManager.StoppableFixedDeltaTime;
        currentAngularVelocity = (rotation - transform.rotation.eulerAngles) / TimeManager.StoppableFixedDeltaTime;
    }
}