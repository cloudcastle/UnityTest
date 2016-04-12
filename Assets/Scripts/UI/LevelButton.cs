using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Text levelName;
    public Text comments;
    
    [NonSerialized]
    Level level;
    Image image;
    Button button;

    Color completedColor = Color.green;
    Color unlockedColor = Color.white;
    Color hidden = new Color(0, 0, 0, 0);

    void Awake() {
        image = GetComponentInChildren<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            GameManager.instance.Play(level);
        });
    }

    public Color GetColor() {
        return level.Completed() ? completedColor : unlockedColor;
    }

    public void SetLevel(Level level) {
        this.level = level;
        levelName.text = level.name;
        if (level.dependencies.Count > 0) {
            comments.text = "Unlocked by: " + level.dependencies.ExtToString(format: "{0}");
        } else {
            comments.text = "";
        }
        levelName.color = GetColor();
        comments.color = hidden;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        comments.color = GetColor();
    }

    public void OnPointerExit(PointerEventData eventData) {
        comments.color = hidden;
    }
}
