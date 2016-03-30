using UnityEngine;
using System.Collections;

public abstract class AbstractTrigger : MonoBehaviour
{
    public Effect effect;

    void Awake() {
        var effect = GetComponent<Effect>();
        if (effect != null) {
            this.effect = effect;
        }
    }
}