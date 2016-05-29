using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Move))]
public class Grounder : MonoBehaviour
{
    Move move;
    ControllerColliderHit lastHit;

    void Awake() {
        move = GetComponent<Move>();
    }

    void Start() {
        new ValueTracker<ControllerColliderHit>(v => lastHit = v, () => lastHit);
    }

    public void ResetGroundHit() {
        lastHit = null;
    }

    void FixedUpdate() {
        if (TimeManager.instance.Undoing()) {
            return;
        }
        if (lastHit != null) {
            ApplyHit(lastHit);
        }
        lastHit = null;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!enabled) {
            return;
        }
        if (TimeManager.Paused) {
            return;
        } 
        lastHit = hit;
    }

    void ApplyHit(ControllerColliderHit hit) {
        if (hit.normal.y > 0.7f) {
            Vector3 normalVelocity = Vector3.Project(move.velocity, Vector3.up);
            Vector3 tangentialVelocity = Vector3.ProjectOnPlane(move.velocity, Vector3.up);

            var movingSurface = hit.gameObject.GetComponentInParent<MovingSurface>();
            if (movingSurface == null) {
                tangentialVelocity = Vector3.zero;
            } else {
                Vector3 radiusVector = hit.point - movingSurface.transform.position;
                //Vector3 rotationVelocity = Vector3.Cross(movingSurface.currentAngularVelocity * Mathf.Deg2Rad, radiusVector);

                //Vector3 localHitPoint = movingSurface.transform.InverseTransformPoint(hit.point);
                Vector3 nextRadiusVector = Quaternion.Euler(movingSurface.currentAngularVelocity * Time.fixedDeltaTime) * radiusVector;
                Vector3 rotationVelocity = (nextRadiusVector - radiusVector) / Time.fixedDeltaTime;

                //Debug.Log("movingSurface.currentAngularVelocity: " + movingSurface.currentAngularVelocity);
                //Debug.Log("rotationVelocity: " + rotationVelocity);

                Vector3 hitPointVelocity = movingSurface.currentVelocity + rotationVelocity;

                tangentialVelocity = hitPointVelocity;

                //Debug.Log("Final tangentialVelocity: " + tangentialVelocity);
            }
            
            move.velocity = normalVelocity + tangentialVelocity;

            move.angularVelocity = movingSurface != null ? movingSurface.currentAngularVelocity : Vector3.zero;
            //move.angularVelocity = move.angularVelocity.Change(x: 0, z: 0);
        }
    }
}