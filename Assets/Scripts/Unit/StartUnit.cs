using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartUnit : Ability
{
    void Start() {
        Player.instance.GainControl(unit);
    }
}