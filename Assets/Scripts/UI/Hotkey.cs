using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Hotkey : MonoBehaviour {
    public string key;
    Button button;

    void Awake() {
        button = GetComponent<Button>();
    }

    void Update() {
        if (Input.GetButtonDown(key)) {
            if (GetComponentInParent<UIScreen>() == LevelUI.instance.CurrentScreen) {
                button.onClick.Invoke();
            }
        }
    }
}
