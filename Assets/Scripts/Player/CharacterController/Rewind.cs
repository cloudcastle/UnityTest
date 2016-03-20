using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Rewind : MonoBehaviour
{
    public float timeMultiplyer = 8;


    public bool On() {
        return Input.GetButton("Rewind");
    }

    void Update() {
        if (TimeManager.paused) {
            return;
        }
        Time.timeScale = (On() ? timeMultiplyer : 1);
    }
}