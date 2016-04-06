using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
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

    public float time;

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

    public static float GameFixedDeltaTime() {
        if (timestopped) {
            return 0;
        } else {
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

    void Update()
    {
        time = Time.time;
    }

    void Awake() {
        Paused = pauseOnStart;
        timestopped = false;
    }
}