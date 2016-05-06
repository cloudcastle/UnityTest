using UnityEngine;
using UnityEngine.Events;

public enum ActivatableStatus
{
    Inactive = 0,
    Activatable = 1,
    Activated = 2
}

public abstract class Activatable : Script
{
    public float maxDistance = 0;
    public float maxBiasAngle = 60;
    public bool useActivatorMaxDistance = true;

    // biased activation only available withn activator max distance
    public bool allowBiasedActivation = true;

    public virtual ActivatableStatus Status() {
        return ActivatableStatus.Activatable;
    }

    public virtual void Activate(Activator activator) {
        if (DebugManager.debug) {
            Debug.Log(string.Format("Activated: {0}", this));
        }
    }

    public float EffectiveMaxDistance(Activator activator) {
        if (useActivatorMaxDistance) {
            return activator.maxDistance;
        }
        return maxDistance;
    }

    public float EffectiveMaxBiasAngle(Activator activator) {
        if (useActivatorMaxDistance) {
            return activator.maxBiasAngle;
        }
        return maxBiasAngle;
    }
}