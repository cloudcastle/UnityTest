using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RSG;
using System;

public class ComposedEffect : Effect
{
    public List<Effect> effects;

    void Awake() {
        effects = this.GetComponentsInDirectChildren<Effect>();
    }

    public override IPromise Run() {
        return Promise.Sequence(effects.Select(effect => (Func<IPromise>)(effect.Run)));
    }
}