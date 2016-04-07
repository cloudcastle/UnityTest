using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationSpeed;
    public bool quick = false;

    float accumulatedTime = 0;

    void FixedUpdate()
    {
        if (TimeManager.Paused) {
            return;
        }
        if (quick) {
            accumulatedTime += TimeManager.GameFixedDeltaTime;
            if (accumulatedTime > TimeManager.loosedFixedDeltaTime - 1e-4) {
                FixedUpdateByTime(accumulatedTime);
                accumulatedTime = 0;
            }
        } else {
            FixedUpdateByTime(TimeManager.GameFixedDeltaTime);
        }
    }

    void FixedUpdateByTime(float time) {

        transform.Rotate(time * rotationSpeed);
    }
}
