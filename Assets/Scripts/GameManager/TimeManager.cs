using UnityEngine;
using System.Collections;
using RSG;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public static IPromiseTimer promiseTimer;

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

    public static float GameTime {
        get {
            if (!Extensions.Editor()) {
                return instance.gameTime;
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

    void FixedUpdate()
    {
        gameTime += Time.fixedDeltaTime;
        promiseTimer.Update(Time.fixedDeltaTime);
    }

    void Awake() {
        instance = this;
        Paused = pauseOnStart;
        timestopped = false;
    }

    void Start() {
        promiseTimer = new UndoablePromiseTimer(() => gameTime);
        Undo.instance.onUndo += OnUndo;
        gameTime = 0;
    }

    void OnUndo() {
        gameTime = Undo.instance.time;
    }

    public static IPromise WaitFor(float time) {
        return promiseTimer.WaitFor(time);
    }

    void Update() {
    }
}