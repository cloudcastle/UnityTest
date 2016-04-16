using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmptyUnitController : UnitController
{
    public static EmptyUnitController instance;

    void Awake() {
        instance = this;
    }

    public override bool Activate() {
        return false;
    }

    public override bool Jump() {
        return false;
    }

    public override Vector2 Mouse() {
        return Vector2.zero;
    }

    public override float Scale() {
        return 0;
    }

    public override bool Crouch() {
        return false;
    }

    public override bool Jetpack() {
        return false;
    }

    public override Vector2 Move() {
        return Vector2.zero;
    }

    public override bool NextItem() {
        return false;
    }

    public override bool PrepareThrow() {
        return false;
    }

    public override bool PreviousItem() {
        return false;
    }

    public override bool Rewind() {
        return false;
    }

    public override bool Run() {
        return false;
    }

    public override bool Throw() {
        return false;
    }

    public override bool ToggleTimestop() {
        return false;
    }

    public override bool Undo() {
        return false;
    }
}