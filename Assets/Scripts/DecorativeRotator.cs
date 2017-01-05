using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorativeRotator : MonoBehaviour {

    public Vector3 angularVelocity;

	void Update() {
        transform.Rotate(angularVelocity, Time.deltaTime * angularVelocity.magnitude, Space.World);	
	}
}
