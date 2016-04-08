using UnityEngine;
using UnityEngine.Events;

public abstract class Activatable : MonoBehaviour
{
    public float maxDistance = 0;
    public bool useActivatorMaxDistance = true;

    // biased activation only available withn activator max distance
    public bool allowBiasedActivation = true;

    public virtual bool Ready() {
        return true;
    }

    public virtual void Activate(Activator activator) {
        Debug.Log(string.Format("Activated: {0}", this));
    }

    public float EffectiveMaxDistance(Activator activator) {
        if (useActivatorMaxDistance) {
            return activator.maxDistance;
        }
        return maxDistance;
    }
}