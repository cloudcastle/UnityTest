using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Cheats : MonoBehaviour
{
    public static bool on = false;

    void Awake() {
        Enable(on);
    }

    void Enable(bool on) {
        Debug.Log(string.Format("Cheats {0}", on ? "on" : "off"));
        Cheats.on = on;
        FindObjectOfType<Jetpack>().enabled = on;

        var map = FindObjectOfType<MapScreen>();
        if (map != null) {
            map.UpdateLevelList();
        }
    }

    void Update() {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F5)) {
            Enable(!on);
        }
        if (on && Input.GetKeyDown(KeyCode.F2)) {
            GameManager.game = new Game();
        }
    }
}