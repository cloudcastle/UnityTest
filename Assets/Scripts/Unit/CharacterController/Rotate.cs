using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Rotate : Ability
{
    public float rotateSpeed = 6.0F;

    void Update()
    {
        if (TimeManager.Paused)
        {
            return;
        }
        transform.Rotate(0, unit.controller.Mouse().x * GameManager.game.settings.mouseSpeed * rotateSpeed, 0, Space.Self);
    }
}