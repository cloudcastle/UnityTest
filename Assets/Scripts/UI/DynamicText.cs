using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class DynamicText : MonoBehaviour
{
    Text text;
    string format;

    void Awake() {
        text = GetComponent<Text>();
        format = text.text;
    }

    void UpdateText() {
        text.text = DynamicTextManager.instance.BuildText(format);
    }

    void Start() {
        UpdateText();
        DynamicTextManager.instance.onInvalidate += UpdateText;
    }
}
