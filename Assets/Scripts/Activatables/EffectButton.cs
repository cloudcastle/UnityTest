using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EffectButton : Activatable
{
    public List<Effect> effects;

    public override void Activate() {
        base.Activate();
        effects.ForEach((effect) => effect.Run());
    }
}