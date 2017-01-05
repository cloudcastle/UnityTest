using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : Pickable {

    public void OnTriggerEnter(Collider c) {
        var picker = c.GetComponentInChildren<Picker>();
        if (picker == null) {
            return;
        }
        picker.Pick(this);
        this.Destroy();
    }
}
