using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Move))]
public class Grounder : MonoBehaviour
{
    Move move;

    void Awake() {
        move = GetComponent<Move>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!enabled) {
            return;
        }
        if (TimeManager.Paused) {
            return;
        }
        if (hit.normal.y > 0.7f) {
            Vector3 normalVelocity = Vector3.Project(move.velocity, hit.normal);
            Vector3 tangentialVelocity = Vector3.ProjectOnPlane(move.velocity, hit.normal);

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
            }
            
            move.velocity = normalVelocity + tangentialVelocity;

            move.angularVelocity = movingSurface != null ? movingSurface.currentAngularVelocity : Vector3.zero;
            //move.angularVelocity = move.angularVelocity.Change(x: 0, z: 0);
        }
    }
}