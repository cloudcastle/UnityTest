using UnityEngine;
using System.Collections;
using RSG;

public abstract class Effect : Script
{
    public abstract IPromise Run();

    public virtual bool Ready() {
        return transform;
    }
}