using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RSG;

public class Ability : Script
{
    public Unit unit;

    public UnitController Controller {
        get { return unit.controller; }
    }

    public override void Awake() {
        base.Awake();
        unit = GetComponentInParent<Unit>();
    }
}