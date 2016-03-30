using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class TimedValue<T>
{
    public T value;
    public float time;

    public TimedValue(T value, float time) {
        this.value = value;
        this.time = time;
    }
}