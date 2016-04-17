using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Undo : Ability
{
    public bool Undoing() {
        return Controller.Undo();
    }
}