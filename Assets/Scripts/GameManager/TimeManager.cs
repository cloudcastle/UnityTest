using UnityEngine;
using System.Collections;
using RSG;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public static IPromiseTimer promiseTimer;

    Substitution timeSubstitution;

    public bool pauseOnStart;
    static bool paused;
    public static bool Paused {
        get {
            return paused;
        }
        set {
            if (value) {
                Pause();
            } else {
                Unpause();
            }
        }
    }
    public static bool timestopped = false;

    [SerializeField]
    float gameTime;

    [SerializeField]
    float stoppableGameTime;

    public static float GameTime {
        get {
            if (!Extensions.Editor()) {
                return instance.gameTime;
            } else {
                return FindObjectOfType<TimeManager>().gameTime;
            }
        }
    }

    public static float StoppableGameTime {
        get {
            if (!Extensions.Editor()) {
                return instance.stoppableGameTime;
            } else {
                return FindObjectOfType<TimeManager>().gameTime;
            }
        }
    }

    public static float loosedFixedDeltaTime;

    static void Pause()
    {
        paused = true;
        Time.timeScale = 0;
    }

    static void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
    }

    public static float StoppableFixedDeltaTime {
        get {
            if (timestopped) {
                return 0;
            } else {
                return Time.fixedDeltaTime;
            }
        }
    }

    public static float FixedDeltaTime {
        get {
            return Time.fixedDeltaTime;
        }
    }

    static void SwitchPause()
    {
        if (Paused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    void FixedUpdate() {
        if (UndoManager.instance.Undoing()) {
            return;
        }
        promiseTimer.Update(-100500);
        gameTime += Time.fixedDeltaTime;
        stoppableGameTime += StoppableFixedDeltaTime;
    }

    void Awake() {
        instance = this;
        Paused = pauseOnStart;
        timestopped = false;
    }

    void Start() {
        promiseTimer = new UndoablePromiseTimer(() => gameTime);
        UndoManager.instance.onUndo += OnUndo;
        gameTime = 0;
        timeSubstitution = DynamicTextManager.instance.Substitute("#{gameTime}", () => {
            var span = TimeSpan.FromSeconds(stoppableGameTime);
            return string.Format("{0}:{1:00}", (int)span.TotalMinutes, span.Seconds);
        });
        new BoolTracker(v => timestopped = v, () => timestopped);
        new ValueTracker<float>(v => stoppableGameTime = gameTime + v, () => stoppableGameTime - gameTime);
    }

    void Update() {
        timeSubstitution.Recalculate();
    }

    void OnUndo() {
        gameTime = UndoManager.instance.time;
    }

    public static IPromise WaitFor(float time) {
        return promiseTimer.WaitFor(time);
    }
}