using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ItemGrounder : MonoBehaviour
{
    Rigidbody rigidBody;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void OnCollisionStay(Collision c)
    {
        if (!enabled) {
            return;
        }
        if (TimeManager.Paused) {
            return;
        }
        var movingSurface = c.gameObject.GetComponent<MovingSurface>();
        if (movingSurface != null) {
            var relativeVelocity = movingSurface.currentVelocity - rigidBody.velocity;
            //Debug.LogFormat("Relative velocity: {0}", relativeVelocity);
            rigidBody.AddForce(relativeVelocity, ForceMode.VelocityChange);
        }
    }
}