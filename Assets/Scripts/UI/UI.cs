using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
    public static UI instance;

    public CompletionScreen completionScreen;
    public MapScreen map;

    public UIScreen currentScreen;

    void Awake() {
        instance = this;
    }

    void HideAllUIScreens() {
        completionScreen.Hide();
        map.Hide();
    }

    void Show(UIScreen screen) {
        HideAllUIScreens();
        screen.Show();
        TimeManager.paused = true;
        currentScreen = screen;
    }

    public void CompletionScreen() {
        Show(completionScreen);
    }

    public void Map() {
        Show(map);
    }
}
