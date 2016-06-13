using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class UnitController : MonoBehaviour
{
    public event Action<Unit> onGainControl = (u) => { };
    public event Action<Unit> onLoseControl = (u) => { };

    public void GainControl(Unit unit) {
        if (unit.controller != null) {
            unit.controller.onLoseControl(unit);
        }
        unit.controller = this;
        this.onGainControl(unit);
    }

    public void LoseControl(Unit unit) {
        unit.controller = EmptyUnitController.instance;
        this.onLoseControl(unit);
        unit.controller.onGainControl(unit);
    }

    public abstract bool Activate();

    public abstract Vector2 Mouse();

    public abstract bool Jump();
    public abstract bool Jetpack();

    public abstract float Scale();

    public abstract bool Crouch();

    public abstract bool Rewind();

    public abstract bool SlowmoSwitch();

    public abstract bool Run();

    public abstract bool PrepareThrow();

    public abstract bool Throw();

    public abstract bool ToggleTimestop();

    public abstract Vector2 Move();

    public abstract bool Undo();

    public abstract bool NextItem();

    public abstract bool PreviousItem();

    public abstract bool BlinkPrepare();

    public abstract bool Blink();
}