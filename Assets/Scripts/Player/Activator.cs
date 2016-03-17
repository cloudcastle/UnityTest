using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour
{
    public static Activator instance;

    public float maxDistance = 2;

    void Awake() {
        instance = this;
    }

    public Activatable current;
    public Activatable outOfRange;

    void Reset() {
        outOfRange = null;
        current = null;
    }

    void Check(Activatable target) {
        if (Eye.instance.distance < target.EffectiveMaxDistance()) {
            current = target;
        } else {
            outOfRange = target;
        }
    }

    void Update()
    {
        if (TimeManager.paused)
        {
            return;
        }
        Reset();
        if (Eye.instance.underSight != null) {
            var target = Eye.instance.underSight.GetComponent<Activatable>();
            if (target != null) {
                Check(target);
            }
        }
        if (Input.GetButtonDown("Activate")) {
            if (current != null) {
                current.Activate();
            }
        }
    }
}