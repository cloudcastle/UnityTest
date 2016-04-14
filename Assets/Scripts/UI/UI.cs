using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UI : MonoBehaviour {
    public static UI instance;

    public ConfirmationScreen confirmationScreen;
    public OptionsScreen optionsScreen;

    public virtual void Awake() {
        instance = this;
    }

    public UIScreen CurrentScreen {
        get {
            if (screenStack.Count == 0) {
                return null;
            }
            return screenStack.Peek();
        }
    }

    public Stack<UIScreen> screenStack = new Stack<UIScreen>();

    protected virtual void HideAllUIScreens() {
        optionsScreen.Hide();
    }

    protected void Show(UIScreen screen) {
        HideAllUIScreens();
        screen.Show();
        TimeManager.Paused = true;
        screenStack.Clear();
        screenStack.Push(screen);
    }

    protected void ShowModal(UIScreen screen) {
        screen.Show();
        TimeManager.Paused = true;
        screenStack.Push(screen);
    }

    public void HideModal() {
        CurrentScreen.Hide();
        screenStack.Pop();
        TimeManager.Paused = screenStack.Count > 0;
    }

    public void OptionsScreen() {
        ShowModal(optionsScreen);
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
}
