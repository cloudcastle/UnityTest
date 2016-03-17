using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CompletionScreen : UIScreen {
    public Text levelName;

    public override void Show() {
        base.Show();
        levelName.text = GameManager.instance.CurrentLevel().name;
    }
}
