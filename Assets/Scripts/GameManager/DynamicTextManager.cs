using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class DynamicTextManager : MonoBehaviour
{
    public static DynamicTextManager instance;

    public List<Substitution> substitutions = new List<Substitution>();

    public void Invalidate() {
        onInvalidate();
    }

    public event Action onInvalidate = () => { };

    public Substitution Substitute(string marker, Func<string> value) {
        var substitution = new Substitution(marker, value);
        substitutions.Add(substitution);
        Invalidate();
        return substitution;
    }

    void Awake() {
        instance = this;
    }

    public string BuildText(string format) {
        var result = format;
        for (int i = 0; i < 100; i++) {
            var next = result;

            next = next.Replace("#{currentLevel}", GameManager.instance.CurrentLevel() == null ? SceneManager.GetActiveScene().name : GameManager.instance.CurrentLevel().name);
            substitutions.ForEach(s => next = s.Use(next));
            if (next == result) {
                break;
            }
            result = next;
        }
        return result;
    }
}