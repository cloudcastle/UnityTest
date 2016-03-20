using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Cheats : MonoBehaviour
{
    bool cheatsEnabled = false;

    void Enable(bool on) {
        Debug.Log(string.Format("Cheats {0}", on ? "on" : "off"));
        cheatsEnabled = on;
        FindObjectOfType<Jetpack>().enabled = on;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F5)) {
            Enable(!cheatsEnabled);
        }
    }
}