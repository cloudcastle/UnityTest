using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour
{
    public float maxDistance = 2;

    public Cooldown stun = new Cooldown(0.2f);

    public Player player;
    
    public Activatable current;
    public float currentBiasAngle;

    const int MAX_SPHERE_CAST_RESULTS = 100;
    Collider[] sphereCastResults = new Collider[MAX_SPHERE_CAST_RESULTS];
    int activatableLayerMask;
    RaycastHit hit;

    void Awake() {
         activatableLayerMask = LayerMask.NameToLayer("Ghost") | LayerMask.NameToLayer("Item");
    }

    void Reset() {
        current = null;
        currentBiasAngle = float.PositiveInfinity;
    }

    void Check(Activatable target, float biasAngle) {
        if (biasAngle < currentBiasAngle) {
            current = target;
            currentBiasAngle = biasAngle;
        }
    }

    private void LocateCurrentActivatable() {
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, maxDistance, sphereCastResults);
        if (cnt >= MAX_SPHERE_CAST_RESULTS) {
            Debug.LogError("MAX_SPHERE_CAST_RESULTS exceeded");
        }
        for (int i = 0; i < cnt; i++) {
            var radiusVector = sphereCastResults[i].transform.position - transform.position;
            if (Vector3.Dot(radiusVector, transform.forward) > 0) {
                Physics.Raycast(transform.position, radiusVector, out hit);
                if (hit.collider.gameObject == sphereCastResults[i].gameObject && hit.distance < maxDistance) {
                    var activatable = hit.collider.GetComponent<Activatable>();
                    Check(activatable, Vector3.Angle(transform.forward, radiusVector));
                }
            }
        }
    }

    void Update()
    {
        if (TimeManager.Paused)
        {
            return;
        }
        Reset();
        if (stun.OnCooldown()) {
            return;
        }
        LocateCurrentActivatable();
        if (Input.GetButtonDown("Activate")) {
            if (current != null) {
                current.Activate(this);
            }
        }
    }
}