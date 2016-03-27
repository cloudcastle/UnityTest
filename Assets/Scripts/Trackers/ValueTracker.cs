using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public abstract class ValueTracker<T>
{
    public int sampleCount;

    Stack<TimedValue<T>> track = new Stack<TimedValue<T>>();

    IEqualityComparer<T> comparer;
    Action<T> setValue;
    Func<T> getValue;

    public ValueTracker(IEqualityComparer<T> comparer, Action<T> setValue, Func<T> getValue) {
        this.comparer = comparer;
        this.setValue = setValue;
        this.getValue = getValue;
        Undo.instance.onUndo += PerformUndo;
        Undo.instance.onTrack += Track;
        Undo.instance.onPushSampleCount += PushSampleCount;
    }

    void Track() {
        if (track.Count > 0 && comparer.Equals(track.Peek().value, getValue())) {
            return;
        } 
        track.Push(new TimedValue<T>(getValue(), Undo.instance.time));
        sampleCount = track.Count;
    }

    void PerformUndo() {
        while (track.Count > 1 && track.Peek().time > Undo.instance.time) {
            track.Pop();
        }
        setValue(track.Peek().value);
        sampleCount = track.Count;
    }

    void PushSampleCount() {
        Undo.instance.totalSampleCount += sampleCount;
    }
}