using UnityEngine;
using System.Collections.Generic;

public class Jumper : MonoBehaviour
{
    public float minVelocityCap = 0.5f;
    public float velocity = 15f;

    void OnTriggerStay(Collider other) {
        if (other.gameObject.GetComponent<Unit>() != null) {
            var move = other.gameObject.GetComponent<Move>();
            var normalVelocity = Vector3.Project(move.velocity, transform.up);
            var tangentVelocity = Vector3.ProjectOnPlane(move.velocity, transform.up);
            if (Vector3.Dot(normalVelocity, transform.up) < minVelocityCap) {
                normalVelocity = transform.up * velocity;
                move.velocity = tangentVelocity + normalVelocity;
                if (other.gameObject.GetComponent<Grounder>() != null) {
                    other.gameObject.GetComponent<Grounder>().ResetGroundHit();
                }
            }
        }
    }
}

