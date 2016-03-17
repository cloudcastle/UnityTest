using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour
{
    public static Activator instance;

    public float maxDistance = 2;

    void Awake() {
        instance = this;
    }

    public Activatable activatable;

    void Check(Activatable target) {
        if (Eye.instance.distance < target.EffectiveMaxDistance()) {
            activatable = target;
        }
    }

    void Update()
    {
        if (PauseManager.paused)
        {
            return;
        }
        activatable = null;
        if (Eye.instance.underSight != null) {
            var target = Eye.instance.underSight.GetComponent<Activatable>();
            if (target != null) {
                Check(target);
            }
        }
        if (Input.GetButtonDown("Activate")) {
            if (activatable != null) {
                activatable.Activate();
            }
        }
    }
}