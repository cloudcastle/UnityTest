using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(CharacterController))]
public class Rotate : MonoBehaviour
{
    public float rotateSpeed = 6.0F;

    void Update()
    {
        if (TimeManager.Paused)
        {
            return;
        }
        transform.Rotate(0, Input.GetAxis("Mouse X") * GameManager.game.settings.mouseSpeed * rotateSpeed, 0, Space.World);
    }
}