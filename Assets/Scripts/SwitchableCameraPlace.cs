using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableCameraPlace : MonoBehaviour {
    public void Apply() {
        Camera.main.transform.SetParent(transform, worldPositionStays: false);
    }
}
