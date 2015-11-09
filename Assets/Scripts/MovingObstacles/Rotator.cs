using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationSpeed;

    void Update()
    {
        transform.Rotate(Time.deltaTime * rotationSpeed);
    }
}
