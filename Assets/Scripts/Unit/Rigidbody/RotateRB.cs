using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody))]
public class RotateRB : AbilityRB
{
    public float rotateSpeed = 6.0F;

    void Update()
    {
        if (TimeManager.Paused)
        {
            return;
        }
        rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, unit.controller.Mouse().x * GameManager.game.settings.mouseSpeed * rotateSpeed, 0)));
        //transform.Rotate(0, unit.controller.Mouse().x * GameManager.game.settings.mouseSpeed * rotateSpeed, 0, Space.World);
    }
}