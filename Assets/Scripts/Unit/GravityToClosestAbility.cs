using UnityEngine;
using System.Collections;
using System.Linq;

public class GravityToClosestAbility : AbilityRB
{
    public bool orderedRotation;

    public float speed;

    public Move move;

    public GameObject lastClosest;

    public override void Start() {
        base.Start();
        this.move = GetComponent<Move>();
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            orderedRotation = true;
        }
    }

    void FixedUpdate() {
        var eps = 1e-4f;

        var distanceToClosest = Algorithms.BinarySearch(0, 1000, d => {
            SpaceScanner.count = Physics.SphereCastNonAlloc(transform.position, d, Vector2.right, SpaceScanner.rayCastResults, 0);
            return SpaceScanner.count <= 1;
        }, eps);
        SpaceScanner.count = Physics.SphereCastNonAlloc(transform.position, distanceToClosest + 2 * eps, Vector2.right, SpaceScanner.rayCastResults, 0);

        var closest = SpaceScanner.rayCastResults.Take(SpaceScanner.count).First(hit => hit.collider != null && hit.collider.gameObject != this.gameObject);
        var closestPointOnBounds = closest.collider.ClosestPointOnBounds(transform.position);
        var movingToClosest = (closest.collider.transform.position - transform.position).normalized;

        SpaceScanner.count = Physics.SphereCastNonAlloc(transform.position - movingToClosest, distanceToClosest + 4 * eps, movingToClosest, SpaceScanner.rayCastResults, 2);
        //var closestHitPoint = SpaceScanner.rayCastResults.Take(SpaceScanner.count).First(hit => hit.collider == closest.collider);

        var newDown = (closest.collider.transform.position - transform.position).normalized;
//        var newDown = (closestHitPoint.point - transform.position).normalized;
        var down = transform.TransformDirection(Vector3.down);
        if (Vector3.Angle(down, newDown) > 0) {
            var rotationAxe = Vector3.Cross(down, newDown);
            rotationAxe = Vector3.ProjectOnPlane(rotationAxe, down);
            var globalVelocity = transform.TransformVector(move.velocity);
            transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.FromToRotation(down, newDown), 0.1f) * transform.rotation;
            move.velocity = transform.InverseTransformVector(globalVelocity);
            if (lastClosest != closest.collider.gameObject) {
                move.velocity = Vector3.zero;
                lastClosest = closest.collider.gameObject;
                Debug.LogFormat("Zero velocity");
            }
        }
    }
}