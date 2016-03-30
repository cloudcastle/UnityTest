using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour
{
    public float maxDistance = 2;

    public Player player;
    
    public Activatable current;
    public Activatable outOfRange;

    void Reset() {
        outOfRange = null;
        current = null;
    }

    void Check(Activatable target) {
        if (player.eye.distance < target.EffectiveMaxDistance(this)) {
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
        if (player.eye.underSight != null) {
            var target = player.eye.underSight.GetComponent<Activatable>();
            if (target != null) {
                Check(target);
            }
        }
        if (Input.GetButtonDown("Activate")) {
            if (current != null) {
                current.Activate(this);
            }
        }
    }
}