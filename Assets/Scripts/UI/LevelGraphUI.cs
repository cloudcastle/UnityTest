using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LevelGraphUI : UI {
    new public static LevelGraphUI instance;

    public StatusPanel statusPanel;

    public override void Awake() {
        base.Awake(); 
        instance = this;
    }

    void Update() {
        //statusPanel.gameObject.SetActive(CameraControl.instance.hovered != null);
    }
}
