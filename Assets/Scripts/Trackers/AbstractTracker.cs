using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class AbstractTracker<T> : Script
{
    public ValueTracker<T> tracker;

    public bool useInitialValue = false;
    public T initialValue;

    protected abstract ValueTracker<T> CreateTracker();

    public override void InitInternal() {
        if (tracker != null) {
            return;
        }
        tracker = CreateTracker();
        if (useInitialValue) {
            Debug.Log("Initial value used: " + this);
            tracker.Init(initialValue);
        }
    }
}