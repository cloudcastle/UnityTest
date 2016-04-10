using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class ValueTracker<T>
{
    public int sampleCount;

    [SerializeField]
    Stack<TimedValue<T>> track = new Stack<TimedValue<T>>();

    Action<T> setValue;
    Func<T> getValue;
    Func<T, bool> isActual;

    public ValueTracker(Action<T> setValue, Func<T> getValue, IEqualityComparer<T> comparer = null, bool useInitialValue = false, T initialValue = default(T)) 
        : this(setValue, getValue, (v) => (comparer ?? EqualityComparer<T>.Default).Equals(v, getValue()), useInitialValue, initialValue) {
    }

    public ValueTracker(Action<T> setValue, Func<T> getValue, Func<T, bool> isActual, bool useInitialValue = false, T initialValue = default(T)) {
        this.setValue = setValue;
        this.getValue = getValue;
        this.isActual = isActual;
        Undo.instance.onUndo += PerformUndo;
        Undo.instance.onTrack += Track;
        Undo.instance.onPushSampleCount += PushSampleCount;
        Undo.instance.onDrop += new Action(Drop);
    }

    public void Init(T value = default(T)) {
        if (track.Count == 0) {
            track.Push(new TimedValue<T>(value, float.NegativeInfinity));
            sampleCount = track.Count;
            if (this is ItemTracker) {
                Debug.Log("ItemTracker track init: " + track.ExtToString());
            }
        }
    }

    public void Drop() {
        track = new Stack<TimedValue<T>>();
        sampleCount = track.Count;
        Track();
    }

    void Track() {
        //if (this is ItemTracker) {
        //    Debug.Log("ItemTracker track: " + track.ExtToString());
        //}
        if (track.Count > 0 && isActual(track.Peek().value)) {
            //if (this is ItemTracker) {
            //    Debug.Log("ItemTracker consider actual: " + (track.Count > 0 ? track.Peek().value : default(T)));
            //}
            return;
        }
        //if (this is ItemTracker) {
        //    Debug.Log("ItemTracker consider not actual: " + (track.Count > 0 ? track.Peek().value : default(T)));
        //}
        track.Push(new TimedValue<T>(getValue(), Undo.instance.time));
        sampleCount = track.Count;
    }

    void PerformUndo() {
        while (track.Count > 1 && track.Peek().time > Undo.instance.time) {
            track.Pop();
        }
        if (track.Count == 0) {
            Init();
        }
        setValue(track.Peek().value);
        sampleCount = track.Count;
    }

    void PushSampleCount() {
        Undo.instance.totalSampleCount += sampleCount;
    }
}