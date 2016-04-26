using UnityEngine;
using System.Collections;

public class CameraPlace : Ability
{
    public float verticalRotateSpeed = 6.0F;

    public TimedValue<Vector3> lastPosition;
    public TimedValue<Vector3> secondLastPosition;

    void Start() {
        lastPosition = new TimedValue<Vector3>(transform.parent.position, Time.realtimeSinceStartup);
        secondLastPosition = new TimedValue<Vector3>(transform.parent.position, float.NegativeInfinity);
    }

    void LateUpdate()
    {
        //transform.position = transform.parent.position + (lastPosition.value - secondLastPosition.value) / (lastPosition.time - secondLastPosition.time) * (Time.realtimeSinceStartup - lastPosition.time);
    }

    void FixedUpdate() {
        secondLastPosition = lastPosition;
        lastPosition = new TimedValue<Vector3>(transform.parent.position, Time.realtimeSinceStartup);
    }
}