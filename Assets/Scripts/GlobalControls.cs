using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControls : MonoBehaviour {
    public float moveSpeed = 2;
    public float rotationSpeed = 20;
    public float rollingSpeed = 2;
    public float radius = 0.5f;
    public float rollingForce = 1;
    public float mouseRollingForce = 1;

    void Update() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.position += Vector3.back * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += Vector3.left * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.I)) {
            transform.Rotate(Vector3.right, Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.K)) {
            transform.Rotate(Vector3.right, -Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.J)) {
            transform.Rotate(Vector3.up, -Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.L)) {
            transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.U)) {
            transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.O)) {
            transform.Rotate(Vector3.forward, -Time.deltaTime * rotationSpeed);
        }

        //if (Input.GetKey(KeyCode.W)) {
        //    GetComponent<Rigidbody>().AddTorque(Vector3.right * Time.deltaTime * moveSpeed / radius * Mathf.Rad2Deg, ForceMode.Force);
        //    transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
        //    transform.Rotate(Vector3.right, Time.deltaTime * moveSpeed / radius * Mathf.Rad2Deg, Space.World);
        //}
        //if (Input.GetKey(KeyCode.S)) {
        //    transform.position += Vector3.back * Time.deltaTime * moveSpeed;
        //    transform.Rotate(Vector3.right, -Time.deltaTime * moveSpeed / radius * Mathf.Rad2Deg, Space.World);
        //}
        //if (Input.GetKey(KeyCode.A)) {
        //    transform.position += Vector3.left * Time.deltaTime * moveSpeed;
        //    transform.Rotate(Vector3.forward, Time.deltaTime * moveSpeed / radius * Mathf.Rad2Deg, Space.World);
        //}
        //if (Input.GetKey(KeyCode.D)) {
        //    transform.position += Vector3.right * Time.deltaTime * moveSpeed;
        //    transform.Rotate(Vector3.forward, -Time.deltaTime * moveSpeed / radius * Mathf.Rad2Deg, Space.World);
        //}

        var av = GetComponent<Rigidbody>().angularVelocity;
        av = transform.InverseTransformVector(av);

        if (Input.GetKey(KeyCode.W)) {
            RollForce(Vector3.right);
        }
        if (Input.GetKey(KeyCode.S)) {
            RollForce(-Vector3.right);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetMouseButton(0)) {
            RollForce(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.E) || Input.GetMouseButton(1)) {
            RollForce(-Vector3.forward);
        }
        if (Input.GetKey(KeyCode.A)) {
            RollForce(-Vector3.up);
        }
        if (Input.GetKey(KeyCode.D)) {
            RollForce(Vector3.up);
        }

        if (av.magnitude > 1) {
            av = av.normalized;
        }

        RollForce(-av / 20);

        GetComponent<Rigidbody>().AddRelativeTorque(Vector3.left * Time.deltaTime * mouseRollingForce * Input.GetAxis("Mouse Y") / radius * Mathf.Rad2Deg, ForceMode.Force);
        GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * Time.deltaTime * mouseRollingForce * Input.GetAxis("Mouse X") / radius * Mathf.Rad2Deg, ForceMode.Force);
    }

    void RollForce(Vector3 around) {
        GetComponent<Rigidbody>().AddRelativeTorque(around * Time.deltaTime * rollingForce / radius * Mathf.Rad2Deg, ForceMode.Force);
    }

    //void Roll(Vector3 around) {
    //    transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
    //    transform.Rotate(Vector3.right, Time.deltaTime * moveSpeed / radius * Mathf.Rad2Deg, Space.World);
    //}
}
