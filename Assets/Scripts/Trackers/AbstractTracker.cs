using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class AbstractTracker<T> : MonoBehaviour
{
    ValueTracker<T> tracker;

    protected abstract ValueTracker<T> CreateTracker();

    void Start() {
        tracker = CreateTracker();
    }
}