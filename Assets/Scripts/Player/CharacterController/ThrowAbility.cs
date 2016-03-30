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

    public void UpdateForce() {
        UI.instance.ShowForce(currentForce / maxForce);
    }

    public void Throw() {
        var target = player.inventory.selected;
        player.inventory.Throw(target);
        target.GetComponent<Item>().Throw(player);
        target.transform.position = player.eye.transform.position + player.eye.transform.forward * initialDistance;
        target.GetComponent<Rigidbody>().AddForce(currentForce * player.eye.transform.forward);

        Debug.Log(String.Format("Thrown {0} at force {1}", target, currentForce));

        currentForce = 0;
        throwing = false;
        UpdateForce();
    }

    void Update() {
        if (TimeManager.paused) {
            return;
        }
        if (player.inventory.selected == null) {
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
        if (TimeManager.paused) {
            currentForce = 0;
            throwing = false;
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