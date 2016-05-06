using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using RSG;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    public static bool debug = false;
    static bool debugOneFrame = false;
    static bool oldDebug = false;

    public static int cnt = 0;

    void Awake() {
        instance = this;

#if UNITY_EDITOR
        Promise.EnablePromiseTracking = true;
        Promise.UnhandledException += Promise_UnhandledException;
#endif
    }

#if UNITY_EDITOR

    public Map<Collider, Map<Collider, bool>> ignoredCollisions = new Map<Collider, Map<Collider, bool>>(() => new Map<Collider, bool>(() => false));

    public List<string> levels;
    public List<string> availableLevels;
    public List<string> completedLevels;

    public List<string> levelsUnlockOrders;

    private void Promise_UnhandledException(object sender, ExceptionEventArgs e) {
        Debug.LogError(String.Format("An unhandled promises exception occured: {0}", e.Exception));
    }

    [ContextMenu("Recalculate debug output data")]
    void RecalculateDebugOutputData() {
        levels = GameManager.game.levels.Select(level => level.name).ToList();
        availableLevels = GameManager.game.AvailableLevelsInUnlockOrder().Select(level => level.name).ToList();
        completedLevels = GameManager.game.completedLevels.Select(level => level.name).ToList();

        levelsUnlockOrders = GameManager.game.levels.Select(level => String.Format("{0} unlocked at {1}", level, level.UnlockOrder())).ToList();
    }

    void Update() {
        if (debugOneFrame) {
            Debug.LogFormat("cnt = {0}", DebugManager.cnt);
            debugOneFrame = false;
            debug = oldDebug;
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.LogFormat("Pending promises: {0}", Promise.GetPendingPromises().ExtToString());
            ignoredCollisions.ForEach(kv => {
                kv.Value.ForEach(kv2 => {
                    if (kv2.Value) {
                        Debug.LogFormat("Ignored collision: {0}, {1}", kv.Key.transform.Path(), kv2.Key.transform.Path());
                    }
                });
            });
        }
        if (Input.GetKeyDown(KeyCode.F11)) {
            FindObjectsOfType<LinkScript>().ToList().ForEach(link => {
                link.AssertAcceptable();
            });
            oldDebug = debug;
            debug = true;
            debugOneFrame = true;
        }
        if (Input.GetKeyDown(KeyCode.F10)) {
            debug ^= true;
            Debug.LogFormat("Debug: {0}", debug);
        }
        DebugManager.cnt = 0;
    }
#endif
}