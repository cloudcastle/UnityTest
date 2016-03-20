using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class EffectButton : Activatable
{
    public List<Effect> effects;

    public override void Activate() {
        base.Activate();
        effects.ForEach((effect) => effect.Run());
    }

    public override bool Ready() {
        return effects.Any(effect => effect.Ready());
    }
}