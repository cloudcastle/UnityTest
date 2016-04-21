using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class EditManager : MonoBehaviour
{
    public bool on = false;

    void Start() {
        Enable(on);
    }

    void Enable(bool on) {
        Debug.Log(string.Format("Editing {0}", on ? "on" : "off"));
        this.on = on;
    }

    void Update() {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F12)) {
            Enable(!on);
        }
    }
}