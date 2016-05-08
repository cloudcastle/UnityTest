using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Cheats : MonoBehaviour
{
    public static bool on = false;

    void Start() {
        Enable(on);
    }

    void Enable(bool on) {
        Debug.Log(string.Format("Cheats {0}", on ? "on" : "off"));
        Cheats.on = on;

        if (!GameManager.instance.OnMap()) {
            FindObjectsOfType<Jetpack>().ToList().ForEach(j => j.enabled = on);
            var map = FindObjectOfType<MapScreen>();
            if (map != null) {
                map.UpdateLevelList();
            }
        }
    }

    void Update() {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F5)) {
            Enable(!on);
        }
        if (on) {
            if (Input.GetKeyDown(KeyCode.F2)) {
                GameManager.game = new Game();
                GameManager.instance.PlayLastUnlocked();
            }
            if (Input.GetKeyDown(KeyCode.F12)) {
                GameManager.instance.CompleteLevel();
            }
        }
    }
}