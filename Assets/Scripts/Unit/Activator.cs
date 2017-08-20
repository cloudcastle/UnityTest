using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

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

    public UnityEvent onActivate;

    public override void Awake() {
        base.Awake();
        activatableLayerMask = LayerMask.GetMask("Ghost", "Item", "Activatable", "Unit");
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
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, 10000f, sphereCastResults, activatableLayerMask);
        if (cnt >= MAX_SPHERE_CAST_RESULTS) {
            Debug.LogError("MAX_SPHERE_CAST_RESULTS exceeded");
        }
        for (int i = 0; i < cnt; i++) {
            var activatable = sphereCastResults[i].GetComponent<Activatable>();
            if (activatable == null) {
                continue;
            }
            var radiusVector = activatable.transform.position - transform.position;
            var biasAngle = Vector3.Angle(transform.forward, radiusVector);
            if (biasAngle < activatable.EffectiveMaxBiasAngle(this)) {
                Physics.Raycast(transform.position, radiusVector, out hit, float.PositiveInfinity, ~LayerMask.GetMask("UnitProof"));
                if (hit.collider != null && hit.collider.gameObject == sphereCastResults[i].gameObject) {
                    if (activatable != null && hit.distance < activatable.EffectiveMaxDistance(this)) {
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
        if (stun && stun.OnCooldown()) {
            return;
        }
        LocateCurrentActivatable();
        if (unit.controller.Activate()) {
            if (current != null) {
                current.Activate(this);
                onActivate.Invoke();
            }
        }
    }
}