using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    public Vector3 maxShift;
    public float period;

    public Vector4 lastPosition;

    void Awake() {
        lastPosition = transform.position.Timed(Time.time);
    }

    Vector3 Delta(float t) {
        return maxShift * Mathf.Sin(t/period);
    }

	void Update() {
        Vector3 center = lastPosition.xyz() - Delta(lastPosition.w);
        transform.position = center + Delta(Time.time);
        lastPosition = transform.position.Timed(Time.time);
	}
}
