using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class Substitution
{
    string marker;
    Func<string> value;

    public Substitution(string marker, Func<string> value) {
        this.marker = marker;
        this.value = value;
    }

    public string Use(string text) {
        return text.Replace(marker, value());
    }
}
