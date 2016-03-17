using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    Text text;

    void Awake() {
        text = GetComponentInChildren<Text>();
    }

    public void SetLevel(string levelName) {
        text.text = levelName;
    }
}
