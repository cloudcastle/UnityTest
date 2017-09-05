using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Cheats : MonoBehaviour
{
    public static bool on = false;

    public const string cheat = "hack";
    public int score = 0;

    void Start() {
        Enable(on);
    }

    void Enable(bool on) {
        Debug.Log(string.Format("Cheats {0}", on ? "on" : "off"));
        Cheats.on = on;
        DebugManager.debugMessage = string.Format("Cheats {0}", on ? "on" : "off");

        if (!GameManager.instance.OnMap()) {
            FindObjectsOfType<Jetpack>().ToList().ForEach(j => j.enabled = on);
            var map = FindObjectOfType<MapScreen>();
            if (map != null) {
                map.UpdateLevelList();
            }
        }
    }

    void Update() {
        bool correctKey = false;

        if (score == cheat.Length) {
            Enable(!on);
            score = 0;
        } else {
            if (Input.GetKeyDown(KeyCode.H) && cheat[score] == 'h') { 
                score++;
                correctKey = true;
            } 
            if (Input.GetKeyDown(KeyCode.A) && cheat[score] == 'a') { 
                score++;
                correctKey = true;
            } 
            if (Input.GetKeyDown(KeyCode.C) && cheat[score] == 'c') { 
                score++;
                correctKey = true;
            } 
            if (Input.GetKeyDown(KeyCode.K) && cheat[score] == 'k') { 
                score++;
                correctKey = true;
            } 
            if (Input.anyKeyDown && !correctKey) {
                score = 0;
            }
        }
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F5)) {
            Enable(!on);
        }
        if (on) { 
            if (Input.GetKeyDown(KeyCode.F2)) {
                GameManager.game = new Game();
                GameManager.instance.PlayLastUnlocked();
            }
            if (Input.GetKeyDown(KeyCode.F10)) {
                GameManager.instance.CompleteLevel();
            }
        }
    }
}