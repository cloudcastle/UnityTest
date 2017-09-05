using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Measurer : MovingSurface {
    public TimedValue<Vector3> lastPosition;
    public TimedValue<Vector3> lastForward;

    void Awake() {
        SaveCurrentTransform();
    }

    void Start() {
        TimeManager.instance.onUndo += OnUndo;
    }

    void OnUndo() {
        //SaveCurrentTransform();
    }

    void FixedUpdate() {
        MeasureVelocity();
        SaveCurrentTransform();
    }

    private void MeasureVelocity() {
        if (TimeManager.StoppableTimeScale == 0) {
            currentVelocity = Vector3.zero;
            onVelocityChange.Invoke(currentVelocity);
            currentAngularVelocity = Vector3.zero;
            return;
        }
        if (TimeManager.StoppableGameTime < lastPosition.time + 1e-5) {
            return;
        }
        currentVelocity = (transform.position - lastPosition.value) / (TimeManager.StoppableGameTime - lastPosition.time);
        onVelocityChange.Invoke(currentVelocity);
        Vector3 deltaRotation = Quaternion.FromToRotation(lastForward.value, transform.forward).eulerAngles;
        deltaRotation = Extensions.NormalizeAngles(deltaRotation);

        currentAngularVelocity = deltaRotation / (TimeManager.StoppableGameTime - lastForward.time);
    }

    void SaveCurrentTransform() {
        lastPosition = new TimedValue<Vector3>(transform.position, TimeManager.StoppableGameTime);
        lastForward = new TimedValue<Vector3>(transform.forward, TimeManager.StoppableGameTime);
    }
}