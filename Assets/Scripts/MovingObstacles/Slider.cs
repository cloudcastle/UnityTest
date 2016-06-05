using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Slider : TimedMover {
    public Transform a;
    public Transform z;
    public float period;
    public float phase;

    protected override Vector3 PositionByTime(float time) {
        Vector3 center = (a.transform.position + z.transform.position) / 2;
        Vector3 radius = (a.transform.position - z.transform.position) / 2; 
        return center + radius * Mathf.Clamp(1.15f * Mathf.Cos((time + phase) / period * 2 * Mathf.PI), -1, 1);
    }

}
