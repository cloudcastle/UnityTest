using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LevelUI : UI {
    new public static LevelUI instance;

    public CompletionScreen completionScreen;
    public MapScreen map;
    public PauseScreen pauseScreen;
    public EnteringScreen enteringScreen;

    public UnityEngine.UI.Slider forceSlider;

    public override void Awake() {
        base.Awake();
        instance = this;
    }

    protected override void HideAllUIScreens() {
        base.HideAllUIScreens();
        completionScreen.Hide();
        map.Hide();
        pauseScreen.Hide();
    }

    public void CompletionScreen() {
        Show(completionScreen);
        enteringScreen.Hide();
    }

    public void PauseScreen() {
        Show(pauseScreen);
    }

    public void Map() {
        GameManager.game.currentLevel = null;
        GameManager.instance.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameManager.levelGraph);
    }

    public void Game() {
        HideAllUIScreens();
        screenStack.Clear();
        TimeManager.Paused = false;
    }

    public void ShowForce(float force) {
        if (force > 0) {
            forceSlider.gameObject.SetActive(true);
            forceSlider.value = force;
        } else {
            forceSlider.gameObject.SetActive(false);
        }
    }
}
