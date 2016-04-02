using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UI : MonoBehaviour {
    public static UI instance;

    public CompletionScreen completionScreen;
    public MapScreen map;
    public PauseScreen pauseScreen;
    public ConfirmationScreen confirmationScreen;
    public EnteringScreen enteringScreen;
    public OptionsScreen optionsScreen;

    public UIScreen CurrentScreen {
        get {
            if (screenStack.Count == 0) {
                return null;
            }
            return screenStack.Peek();
        }
    }

    public Stack<UIScreen> screenStack = new Stack<UIScreen>();

    public UnityEngine.UI.Slider forceSlider;

    void Awake() {
        instance = this;
    }

    void HideAllUIScreens() {
        completionScreen.Hide();
        map.Hide();
        pauseScreen.Hide();
        optionsScreen.Hide();
    }

    void Show(UIScreen screen) {
        HideAllUIScreens();
        screen.Show();
        TimeManager.paused = true;
        screenStack.Clear();
        screenStack.Push(screen);
    }

    void ShowModal(UIScreen screen) {
        screen.Show();
        TimeManager.paused = true;
        screenStack.Push(screen);
    }

    public void HideModal() {
        CurrentScreen.Hide();
        screenStack.Pop();
        TimeManager.paused = screenStack.Count > 0;
    }

    public void CompletionScreen() {
        Show(completionScreen);
        enteringScreen.Hide();
    }

    public void PauseScreen() {
        Show(pauseScreen);
    }

    public void OptionsScreen() {
        ShowModal(optionsScreen);
    }

    public void Map() {
        Show(map);
        enteringScreen.Hide();
    }

    public void Confirm(Action action, string actionName) {
        ShowModal(confirmationScreen);
        confirmationScreen.Init(
            actionName: actionName,
            yes: () => {
                HideModal();
                action();
            },
            no: () => {
                HideModal();
            }
        );
    }

    public void Game() {
        HideAllUIScreens();
        screenStack.Clear();
        TimeManager.paused = false;
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
