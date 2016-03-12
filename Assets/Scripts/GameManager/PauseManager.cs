using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public bool pauseOnStart;
    public static bool paused = true;

    void Pause()
    {
        paused = true;
        Time.timeScale = 0;
    }

    void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
    }

    void SwitchPause()
    {
        if (paused)
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
        if (Input.GetButtonDown("Cancel/Menu"))
        {
            SwitchPause();
        }
    }

    void Awake() {
        paused = pauseOnStart;
    }
}