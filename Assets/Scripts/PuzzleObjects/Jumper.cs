using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Jumper : MonoBehaviour
{
    public float minVelocityCap = 0.5f;
    public float velocity = 15f;
    public bool autoAlign = false;

    public UnityEvent onJump;

    void OnTriggerStay(Collider other) {
        if (other.gameObject.GetComponent<Unit>() != null) {
            var move = other.gameObject.GetComponent<Move>();
            var normalVelocity = Vector3.Project(move.velocity, transform.up);
            var tangentVelocity = Vector3.ProjectOnPlane(move.velocity, transform.up);
            if (Vector3.Dot(normalVelocity, transform.up) < minVelocityCap) {
                normalVelocity = transform.up * velocity;
                if (autoAlign) {
                    tangentVelocity = Vector3.zero;
                }
                move.velocity = tangentVelocity + normalVelocity;
                if (other.gameObject.GetComponent<Grounder>() != null) {
                    other.gameObject.GetComponent<Grounder>().ResetGroundHit();
                }
                if (autoAlign) {
                    other.transform.position = transform.position;
                }
                onJump.Invoke();
            }
        }
    }
}

