using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TimeStoppable : MonoBehaviour
{
    Vector3 savedVelocity;
    Vector3 savedAngularVelocity;
    Rigidbody rigidBody;

    bool timeStopped = false;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start() {
        new BoolTracker(v => {
            timeStopped = v;
            rigidBody.useGravity = !v;
        }, () => timeStopped);
        new Vector3Tracker(v => savedVelocity = v, () => savedVelocity);
        new Vector3Tracker(v => savedAngularVelocity = v, () => savedAngularVelocity);
    }

    void FixedUpdate() {
        if (!timeStopped && TimeManager.timestopped) {
            savedVelocity = rigidBody.velocity;
            savedAngularVelocity = rigidBody.angularVelocity;
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.useGravity = false;
            timeStopped = true;
        }
        if (TimeManager.timestopped) {
            if (rigidBody.velocity != Vector3.zero) {
                savedVelocity = rigidBody.velocity;
            }
            if (rigidBody.angularVelocity != Vector3.zero) {
                savedAngularVelocity = rigidBody.angularVelocity;
            }
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }
        if (timeStopped && !TimeManager.timestopped) {
            rigidBody.velocity = savedVelocity;
            rigidBody.angularVelocity = savedAngularVelocity;
            rigidBody.useGravity = true;
            timeStopped = false;
        }
    }
}
