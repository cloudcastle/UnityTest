using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingSurface : MonoBehaviour {
    public Vector3 currentVelocity;
    public Vector3 currentAngularVelocity;

    public TimedValue<Vector3> lastPosition;
    public TimedValue<Vector3> lastForward;

    void Awake() {
        SaveCurrentTransform();
    }

    void Start() {
        Undo.instance.onUndo += OnUndo;
    }

    void OnUndo() {
        SaveCurrentTransform();
    }

    void FixedUpdate() {
        MeasureVelocity();
        SaveCurrentTransform();
    }

    private void MeasureVelocity() {
        if (Time.time < lastPosition.time + 1e-5) {
            return;
        }
        currentVelocity = (transform.position - lastPosition.value) / (Time.time - lastPosition.time);
        Vector3 deltaRotation = Quaternion.FromToRotation(lastForward.value, transform.forward).eulerAngles;
        deltaRotation = Extensions.NormalizeAngles(deltaRotation);

        currentAngularVelocity = deltaRotation / (Time.time - lastForward.time);
    }

    void SaveCurrentTransform() {
        lastPosition = new TimedValue<Vector3>(transform.position, Time.time);
        lastForward = new TimedValue<Vector3>(transform.forward, Time.time);
    }
}