using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Predictor : MovingSurface {
    public void Predict(Vector3 position, Vector3 rotation) {
        if (TimeManager.GameFixedDeltaTime < 1e-4) {
            currentVelocity = Vector3.zero;
            currentAngularVelocity = Vector3.zero;
        }
        currentVelocity = (position - transform.position) / TimeManager.GameFixedDeltaTime;
        currentAngularVelocity = (rotation - transform.rotation.eulerAngles) / TimeManager.GameFixedDeltaTime;
    }
}