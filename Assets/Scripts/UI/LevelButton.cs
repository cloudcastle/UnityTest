using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    Level level;
    Text text;

    void Awake() {
        text = GetComponentInChildren<Text>();
        GetComponent<Button>().onClick.AddListener(() => {
            GameManager.instance.Play(level);
        });
    }

    public void SetLevel(Level level) {
        this.level = level;
        text.text = level.name;
    }
}
