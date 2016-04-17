using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class Substitution
{
    string marker;
    Func<string> getValue;
    string currentValue;

    public void Recalculate() {
        var nextValue = getValue();
        if (nextValue != currentValue) {
            currentValue = nextValue;
            Invalidate();
        }
    }

    private void Invalidate() {
        DynamicTextManager.instance.Invalidate();
    }

    public Substitution(string marker, Func<string> getValue) {
        this.marker = marker;
        this.getValue = getValue;
    }

    public string Use(string text) {
        return text.Replace(marker, currentValue);
    }
}
