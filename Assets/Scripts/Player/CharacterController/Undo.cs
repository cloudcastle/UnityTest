using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Undo : MonoBehaviour
{
    public static Undo instance;

    public float time;

    public int totalSampleCount;

    public event Action onUndo = () => { };
    public event Action onTrack = () => { };
    public event Action onPushSampleCount = () => { };
    
    void Awake() {
        instance = this;
    }

    void Update() {
        if (TimeManager.paused) {
            return;
        }
        if (Input.GetButton("Undo")) {
            time -= Time.deltaTime;
            if (time < 0) {
                time = 0;
            }
            onUndo();
        } else {
            time += Time.deltaTime;
            onTrack();
        }
        totalSampleCount = 0;
        onPushSampleCount();
    }
}