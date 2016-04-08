using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class DynamicText : UIScreen
{
    Text text;
    string format;

    void Awake() {
        text = GetComponent<Text>();
        format = text.text;
    }

    void Start() {
        text.text = BuildText();
        Debug.Log("Builded text: " + text.text);
    }

    string BuildText() {
        var result = format;
        for (int i = 0; i < 100; i++) {
            var next = result;
            next = next.Replace("#{lastUnlockedLevel}", GameManager.game.AvailableLevelsInUnlockOrder().Last().name);
            next = next.Replace("#{currentLevel}", GameManager.instance.CurrentLevel() == null ? SceneManager.GetActiveScene().name : GameManager.instance.CurrentLevel().name);
            if (next == result) {
                break;
            }
            result = next;
        }
        return result;
    }
}
