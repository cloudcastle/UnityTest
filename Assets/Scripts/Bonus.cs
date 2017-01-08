using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bonus : Pickable {
    public UnityEvent onPicked;

    public void OnTriggerEnter(Collider c) {
        if (TimeManager.instance.Undoing()) {
            return;
        }
        var picker = c.GetComponentInChildren<Picker>();
        if (picker == null) {
            return;
        }
        picker.Pick(this);
        gameObject.SetActive(false);
        onPicked.Invoke();
    }
}
