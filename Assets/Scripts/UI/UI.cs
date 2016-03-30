using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
    public static UI instance;

    public CompletionScreen completionScreen;
    public MapScreen map;

    public UIScreen currentScreen;

    public UnityEngine.UI.Slider forceSlider;

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

    public void ShowForce(float force) {
        if (force > 0) {
            forceSlider.gameObject.SetActive(true);
            forceSlider.value = force;
        } else {
            forceSlider.gameObject.SetActive(false);
        }
    }
}
