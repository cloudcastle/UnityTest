using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using RSG;

public class DebugManager : MonoBehaviour
{
    public List<string> levels;
    public List<string> availableLevels;
    public List<string> completedLevels;

    public List<string> levelsUnlockOrders;

    private void Promise_UnhandledException(object sender, ExceptionEventArgs e) {
        Debug.LogError(String.Format("An unhandled promises exception occured: {0}", e.Exception));
    }

    void Awake() {
        Promise.EnablePromiseTracking = true;
        Promise.UnhandledException += Promise_UnhandledException;
    }

    [ContextMenu("Recalculate debug output data")]
    void RecalculateDebugOutputData() {
        levels = GameManager.game.levels.Select(level => level.name).ToList();
        availableLevels = GameManager.game.AvailableLevelsInUnlockOrder().Select(level => level.name).ToList();
        completedLevels = GameManager.game.completedLevels.Select(level => level.name).ToList();

        levelsUnlockOrders = GameManager.game.levels.Select(level => String.Format("{0} unlocked at {1}", level, level.UnlockOrder())).ToList();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.LogFormat("Pending promises: {0}", Promise.GetPendingPromises().ExtToString());
        }
        if (Input.GetKeyDown(KeyCode.F11)) {
            FindObjectsOfType<LinkScript>().ToList().ForEach(link => {
                link.AssertAcceptable();
            });
        }
    }
}