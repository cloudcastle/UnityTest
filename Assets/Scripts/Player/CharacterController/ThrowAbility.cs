using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class ThrowAbility : MonoBehaviour
{
    public float maxForce = 1;
    public float fullChargeTime = 1;
    public float currentForce = 0;

    public float initialDistance = 0.5f;
    public bool throwing = false;

    public Player player;

    public float throwDelay = 0.04f;

    public void UpdateForce() {
        UI.instance.ShowForce(currentForce / maxForce);
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
        item.transform.position = player.eye.transform.position + player.eye.transform.forward * initialDistance;
        item.GetComponent<Rigidbody>().AddForce(force * player.eye.transform.forward);
    }

    public void Throw() {
        var target = player.inventory.selected;
        player.inventory.Throw(target);

        var throwForce = currentForce;

        TimeManager.WaitFor(throwDelay).Then(() => {
            PushItem(target, throwForce);
            Debug.Log(String.Format("Thrown {0} at place {1}", target, target.transform.position));
            Reset();
        });
        Reset();
    }

    void Update() {
        if (TimeManager.Paused) {
            return;
        }
        if (player.inventory.selected == null) {
            Reset();
            return;
        } 
        if (throwing && Input.GetButtonUp("Throw")) {
            Throw();
        }
        if (Input.GetButtonDown("Throw")) {
            throwing = true;
        }
    }

    void FixedUpdate() {
        if (TimeManager.Paused) {
            Reset();
            return;
        }
        if (player.inventory.selected == null) {
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