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

    void PushItem(Item item, Vector3 pushPosition, Vector3 force) {
        item.transform.position = pushPosition;
        if (item.GetComponent<LastPositionKeeper>() != null) {
            item.GetComponent<LastPositionKeeper>().Reset();
        }
        item.GetComponent<Rigidbody>().AddForce(force / 50f, ForceMode.Impulse);
        TimeManager.WaitFor(0.01f).Then(() => {
            Debug.LogFormat("Item pushed at velocity {0}", item.GetComponent<Rigidbody>().velocity);
        });
    }

    public void Throw() {
        var target = unit.inventory.selected;
        unit.inventory.Lose(target);
        target.gameObject.SetActive(false);

        var throwForce = currentForce * unit.eye.transform.forward;
        var pushPosition = unit.eye.transform.position + unit.eye.transform.forward * initialDistance;

        TimeManager.WaitFor(throwDelay).Then(() => {
            target.gameObject.SetActive(true);
            PushItem(target, pushPosition, throwForce);
            target.GhostFor(unit);
            Debug.LogFormat("Thrown {0} at place {1}", target, target.transform.position.ExtToString());
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
                //currentForce = maxForce;
            } 
        } 
    }
}