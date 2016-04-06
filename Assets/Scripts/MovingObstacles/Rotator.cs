using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationSpeed;

    void FixedUpdate()
    {
        if (TimeManager.Paused) {
            return;
        }
        transform.Rotate(TimeManager.GameFixedDeltaTime() * rotationSpeed);
    }
}
