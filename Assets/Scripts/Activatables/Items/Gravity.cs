using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    public Vector3 gravity;
    public Rigidbody rigidBody;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start() {
        new Vector3Tracker(v => gravity = v, () => gravity);
    }

    void FixedUpdate() {
        if (!TimeManager.timestopped && !TimeManager.instance.Undoing()) {
            rigidBody.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}
