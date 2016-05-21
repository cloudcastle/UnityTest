using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class UndoManager : MonoBehaviour
{
    public static UndoManager instance;

    public float time { get; private set; }

    public int totalSampleCount;

    public event Action onUndo = () => { };
    public event Action onDrop = () => { };
    public event Action onTrack = () => { };
    public event Action onPushSampleCount = () => { };

    void Awake() {
        instance = this;
    }

    public bool Undoing() {
        return Player.instance.current.undo != null && Player.instance.current.undo.Undoing();
    }

    void FixedUpdate() {
        if (TimeManager.Paused) {
            return;
        }
        if (Undoing()) {
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