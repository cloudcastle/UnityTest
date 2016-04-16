using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RSG;

public class Ability : MonoBehaviour
{
    public Unit unit;

    public UnitController Controller {
        get { return unit.controller; }
    }

    public virtual void Awake() {
        unit = GetComponentInParent<Unit>();
    }
}