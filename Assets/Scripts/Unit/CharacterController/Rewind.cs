using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Rewind : Ability, IRewind
{
    public float timeMultiplyer = 8;

    public override void InitInternal() {
        base.InitInternal();
        TimeManager.instance.rewinds.Add(this);
    }

    public float Rewinding() {
        return Controller.Rewind() ? timeMultiplyer : 1;
    }
}