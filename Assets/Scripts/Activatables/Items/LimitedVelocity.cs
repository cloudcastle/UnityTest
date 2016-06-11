using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LimitedVelocity : MonoBehaviour
{
    public float maxVelocity = 300f;

    Rigidbody rigidBody;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (rigidBody.velocity.magnitude > maxVelocity) {
            rigidBody.velocity = rigidBody.velocity.normalized * maxVelocity;
        }
    }
}
