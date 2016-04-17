using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class ThrowAbility : Ability
{
    public float maxForce = 1;
    public float fullChargeTime = 1;
    public float currentForce = 0;

    public float initialDistance = 0.5f;
    public bool throwing = false;

    float throwDelay = 0.04f;

    public void UpdateForce() {
        LevelUI.instance.ShowForce(currentForce / maxForce);
    }

    void Reset() {
        if (!throwing) {
            return;
        }
        currentForce = 0;
        throwing = false;
        UpdateForce();
    }

    void PushItem(Item item, float force) {
        item.transform.position = unit.eye.transform.position + unit.eye.transform.forward * initialDistance;
        item.GetComponent<Rigidbody>().AddForce(force * unit.eye.transform.forward);
    }

    public void Throw() {
        var target = unit.inventory.selected;
        unit.inventory.Lose(target);
        target.gameObject.SetActive(false);

        var throwForce = currentForce;

        TimeManager.WaitFor(throwDelay).Then(() => {
            target.gameObject.SetActive(true);
            PushItem(target, throwForce);
            target.GhostFor(unit);
            Debug.Log(String.Format("Thrown {0} at place {1}", target, target.transform.position.ExtToString()));
            Reset();
        });
        Reset();
    }

    void Update() {
        if (TimeManager.Paused) {
            return;
        }
        if (unit.inventory.selected == null) {
            Reset();
            return;
        } 
        if (throwing && Controller.Throw()) {
            Throw();
        }
        if (Controller.PrepareThrow()) {
            throwing = true;
        }
    }

    void FixedUpdate() {
        if (TimeManager.Paused) {
            Reset();
            return;
        }
        if (unit.inventory.selected == null) {
            return;
        }
        if (throwing) {
            currentForce += maxForce / fullChargeTime * Time.deltaTime;
            UpdateForce();
            if (currentForce > maxForce) {
                Throw();
            } 
        } 
    }
}