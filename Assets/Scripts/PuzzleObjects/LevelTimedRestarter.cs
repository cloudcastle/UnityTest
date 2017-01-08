﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelTimedRestarter : MonoBehaviour
{
    public float restartTime = 30;
    float sceneLoadedTime;

    public Text timerText;

    void Awake() {
        sceneLoadedTime = Time.realtimeSinceStartup;
    }

    float LevelRealTime() {
        return Time.realtimeSinceStartup - sceneLoadedTime;
    }

    public float RemainingTime() {
        return restartTime - LevelRealTime();
    }

    void Update() {
        if (LevelUI.instance.CurrentScreen == LevelUI.instance.completionScreen) {
            gameObject.SetActive(false);
            return;
        }
        if (RemainingTime() < 0) {
            GameManager.instance.Restart();
        }
        if (LevelRealTime() > 3) {
            timerText.gameObject.SetActive(true);
            timerText.text = string.Format("Level restart in {0}...", Mathf.CeilToInt(RemainingTime()));
        }
    }
}
