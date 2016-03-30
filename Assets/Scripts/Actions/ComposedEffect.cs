using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ComposedEffect : Effect
{
    public List<Effect> effects;

    void Awake() {
        effects = this.GetComponentsInDirectChildren<Effect>();
    }

    public override bool Run() {
        effects.ForEach(effect => effect.Run());
        return true;
    }
}