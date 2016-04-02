using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ConfirmationScreen : UIScreen {
    public Text actionNameText;
    Action yes;
    Action no;

    public void Init(string actionName, Action yes, Action no) {
        this.yes = yes;
        this.no = no;
        actionNameText.text = actionName;
    }

    public void Yes() {
        yes();
    }

    public void No() {
        no();
    }
}
