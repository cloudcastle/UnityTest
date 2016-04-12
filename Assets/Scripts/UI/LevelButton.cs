using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Linq;

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Text levelName;
    public Text comments;
    
    [NonSerialized]
    Level level;
    Image image;
    Button button;
    Hotkey hotkey;

    Color completedColor = Color.green;
    Color unlockedColor = Color.white;
    Color hidden = new Color(0, 0, 0, 0);

    void Awake() {
        image = GetComponentInChildren<Image>();
        button = GetComponent<Button>();
        hotkey = GetComponent<Hotkey>();
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
        hotkey.enabled = level == GameManager.game.AvailableLevelsInReverseUnlockOrder().FirstOrDefault();
        NotUnderPointer();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        UnderPointer();
    }

    public void OnPointerExit(PointerEventData eventData) {
        NotUnderPointer();
    }

    void UnderPointer() {
        comments.color = GetColor();
        comments.text = "Unlocked by: " + level.dependencies.ExtToString(format: "{0}");
    }

    void NotUnderPointer() {
        if (hotkey.enabled) {
            comments.color = GetColor();
            comments.text = "[Space] to play";
        } else {
            comments.color = hidden;
        }
    }
}
