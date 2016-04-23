using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class EffectButton : Activatable
{
    public List<Effect> effects;

    public override void Activate(Activator activator) {
        base.Activate(activator);
        effects.ForEach((effect) => effect.Run());
    }

    public override ActivatableStatus Status() {
        if (effects.Any(effect => effect.Status() == ActivatableStatus.Activatable)) {
            return ActivatableStatus.Activatable;
        };
        if (effects.Any(effect => effect.Status() == ActivatableStatus.Activated)) {
            return ActivatableStatus.Activated;
        };
        return ActivatableStatus.Inactive;
    }
}