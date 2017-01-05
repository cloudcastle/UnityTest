using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Undo : Ability, IUndo
{
    public override void Start() {
        base.Start();
        TimeManager.instance.undos.Add(this);
    }

    public bool Undoing() {
        return Controller.Undo();
    }
}