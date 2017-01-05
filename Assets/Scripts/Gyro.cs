using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour {
    public Transform hangPoint;
    public Vector3 delta;

    void Awake() {
        delta = transform.position - hangPoint.transform.position;
    }

    void Update() {
        transform.position = hangPoint.transform.position + delta;
    }
}
