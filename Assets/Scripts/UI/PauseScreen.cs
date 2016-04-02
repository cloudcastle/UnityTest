using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PauseScreen : UIScreen {
    public Text levelName;

    public override void Show() {
        base.Show();
        levelName.text = GameManager.instance.CurrentLevel().name;
    }
}
