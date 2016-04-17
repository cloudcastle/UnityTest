using UnityEngine;
using UnityEngine.Events;

public class PlayerUnit : Activatable
{
    public Unit target;

    public override void Awake() {
        base.Awake();
        maxDistance = float.PositiveInfinity;
        useActivatorMaxDistance = false;
        allowBiasedActivation = false;
    }

    public override void Activate(Activator activator) {
        base.Activate(activator);
        if (activator.unit.controller == Player.instance) {
            Player.instance.Possess(target);
        }
    }
}