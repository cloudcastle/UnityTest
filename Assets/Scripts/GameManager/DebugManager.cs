using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using RSG;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    public Map<Collider, Map<Collider, bool>> ignoredCollisions = new Map<Collider, Map<Collider, bool>>(() => new Map<Collider, bool>(() => false));

    public static bool debug = false;

    public static int cnt = 0;

    public static List<string> drawnPortals = new List<string>();

    public static string debugMessage = "";
    public static Substitution debugSubstitution;

    public void Message(params string[] ss) {
        debugMessage = String.Join("\n", ss);
        debugSubstitution.Recalculate();
    }

    void Awake() {
        instance = this;

#if UNITY_EDITOR
        Promise.EnablePromiseTracking = true;
        Promise.UnhandledException += Promise_UnhandledException;
#endif
    }

    void Start() {
        debugSubstitution = DynamicTextManager.instance.Substitute("#{debug}", () => debugMessage);
    }

#if UNITY_EDITOR
    static bool debugOneFrame = false;
    static bool oldDebug = false;
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
            Debug.LogFormat("drawnPortals = {0}", DebugManager.drawnPortals.ExtToString());
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
            Debug.LogFormat("Trackers: {0}", ValueTracker<double>.cnt);
        }
        if (Input.GetKeyDown(KeyCode.O)) {
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
        DebugManager.drawnPortals.Clear();
    }

    [ContextMenu("Experiment")]
    void Experiment() {
        var go1 = new GameObject();
        go1.transform.position = new Vector3(-2, 5.5f, 0.25f);
        go1.transform.rotation = new Quaternion(72, 0.18f, 3.13f, -24.9f);
        Debug.LogFormat("Transform is: {0}, localToWorldMatrix is:\n{1}", go1.transform.ExtToString(), go1.transform.localToWorldMatrix);

        //Vector3 point = new Vector3(-11, 17.17f, -32.009f);
        //Debug.LogFormat("Transformed by method: {0}", go1.transform.TransformPoint(point));
        //Debug.LogFormat("Transformed by matrix: {0}", go1.transform.localToWorldMatrix.MultiplyPoint3x4(point));

        Portal firstPortal = FindObjectsOfType<Portal>().First(p => p.name == "Portal 3B");
        Portal secondPortal = FindObjectsOfType<Portal>().First(p => p.name == "Portal 5A");
        Vector3 position0 = go1.transform.position;
        Vector3 position1 = secondPortal.TeleportPoint(firstPortal.TeleportPoint(position0));
        Vector3 position2 = (secondPortal.TeleportMatrix() * firstPortal.TeleportMatrix()).MultiplyPoint3x4(position0);
        Debug.LogFormat("Position1: {0}", position1);
        Debug.LogFormat("Position2: {0}", position2);

        Vector3 direction0 = go1.transform.forward;
        Debug.LogFormat("Direction0: {0}", direction0);
        Vector3 direction1 = secondPortal.TeleportDirection(firstPortal.TeleportDirection(direction0));
        Vector3 direction2 = (secondPortal.TeleportMatrix() * firstPortal.TeleportMatrix()).MultiplyVector(direction0);
        Debug.LogFormat("Direction1: {0}", direction1);
        Debug.LogFormat("Direction2: {0}", direction2);

        DestroyImmediate(go1);
    }
#endif
}