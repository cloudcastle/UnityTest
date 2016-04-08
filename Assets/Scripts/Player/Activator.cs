using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour
{
    public float maxDistance = 2;

    public Player player;
    
    public Activatable current;
    public Activatable outOfRange;

    const int MAX_SPHERE_CAST_RESULTS = 100;
    Collider[] sphereCastResults = new Collider[MAX_SPHERE_CAST_RESULTS];

    public Cooldown stun = new Cooldown(0.2f);

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

    private void LocateCurrentActivatable() {
        if (player.eye.underSight != null) {
            var target = player.eye.underSight.GetComponent<Activatable>();
            if (target != null) {
                Check(target);
            }
        }
    }

    void Update()
    {
        if (TimeManager.Paused)
        {
            return;
        }
        Reset();
        if (stun.OnCooldown()) {
            return;
        }
        LocateCurrentActivatable();
        if (Input.GetButtonDown("Activate")) {
            if (current != null) {
                current.Activate(this);
            }
        }
    }
}