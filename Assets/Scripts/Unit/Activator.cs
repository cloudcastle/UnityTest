using UnityEngine;
using System.Collections;
using System;

public class Activator : Ability
{
    public float maxDistance = 2;

    public Cooldown stun;

    public Activatable current;
    public float currentBiasAngle;

    public float maxBiasAngle = 60f;

    const int MAX_SPHERE_CAST_RESULTS = 100;
    Collider[] sphereCastResults = new Collider[MAX_SPHERE_CAST_RESULTS];
    int activatableLayerMask;
    RaycastHit hit;

    public override void Awake() {
        base.Awake();
        activatableLayerMask = LayerMask.GetMask("Ghost", "Item", "Activatable");
    }

    void Start() {
        stun = new Cooldown(0.2f);
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
        LocateDirectSightActivatable();
        if (current != null) {
            return;
        }
        LocateBiasedActivatable();
    }

    private void LocateDirectSightActivatable() {
        if (unit.eye.underSight != null) {
            var activatable = unit.eye.underSight.GetComponent<Activatable>();
            if (activatable != null) {
                if (unit.eye.distance < activatable.EffectiveMaxDistance(this)) {
                    current = activatable;
                }
            }
        }
    }

    private void LocateBiasedActivatable() {
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, maxDistance, sphereCastResults, activatableLayerMask);
        if (cnt >= MAX_SPHERE_CAST_RESULTS) {
            Debug.LogError("MAX_SPHERE_CAST_RESULTS exceeded");
        }
        for (int i = 0; i < cnt; i++) {
            var radiusVector = sphereCastResults[i].transform.position - transform.position;
            var biasAngle = Vector3.Angle(transform.forward, radiusVector);
            if (biasAngle < maxBiasAngle) {
                Physics.Raycast(transform.position, radiusVector, out hit);
                if (hit.collider == null) {
                    continue;
                }
                if (hit.collider.gameObject == sphereCastResults[i].gameObject && hit.distance < maxDistance) {
                    var activatable = hit.collider.GetComponent<Activatable>();
                    if (activatable != null) {
                        Check(activatable, biasAngle);
                    }
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
        if (unit.controller.Activate()) {
            if (current != null) {
                current.Activate(this);
            }
        }
    }
}