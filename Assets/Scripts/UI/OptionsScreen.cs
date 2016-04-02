using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsScreen : UIScreen {

    public UnityEngine.UI.Slider slider;

    public override void Show() {
        base.Show();
        slider.value = Mathf.Log(GameManager.game.settings.mouseSpeed);
    }

    public void Apply() {
        Debug.Log("Apply options");
        GameManager.game.settings.mouseSpeed = Mathf.Exp(slider.value);
        GameManager.instance.Save();
        UI.instance.HideModal();
    }
}
