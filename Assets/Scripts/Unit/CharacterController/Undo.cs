using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Undo : Ability
{
    public static Undo instance;

    public float time { get; private set; }

    public int totalSampleCount;

    public event Action onUndo = () => { };
    public event Action onDrop = () => { };
    public event Action onTrack = () => { };
    public event Action onPushSampleCount = () => { };
    
    public override void Awake() {
        base.Awake();
        instance = this;
    }

    void FixedUpdate() {
        if (TimeManager.Paused) {
            return;
        }
        if (Controller.Undo()) {
            time -= Time.fixedDeltaTime;
            if (time < 0) {
                time = 0;
            }
            onUndo();
        } else {
            time += Time.fixedDeltaTime;
            Track();
        }
        totalSampleCount = 0;
        onPushSampleCount();
    }

    public void Track() {
        onTrack();
    }

    /// <summary>
    /// Make it "It's always been like this" for current state of game
    /// </summary>
    public void DropUndoData() {
        onDrop();
        Debug.Log("Drop Undo Data");
    }
}