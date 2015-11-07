using UnityEngine;
using System.Collections;

public class KeyboardRotator : MonoBehaviour {
    float rotationSpeed = 100;

	void Update () {
        transform.Rotate(Vector3.up, Time.deltaTime * Input.GetAxis("Horizontal") * rotationSpeed);
	}
}
