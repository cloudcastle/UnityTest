using UnityEngine;
using System.Collections;

public class CameraPlace : Ability
{
    public TimedValue<Vector3> lastPosition;
    public TimedValue<Vector3> secondLastPosition;

    public override void InitInternal() {
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