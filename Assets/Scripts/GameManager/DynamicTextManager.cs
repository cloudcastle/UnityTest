﻿using UnityEngine;
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

    public void Substitute(string marker, Func<string> value) {
        substitutions.Add(new Substitution(marker, value));
        Invalidate();
    }

    void Awake() {
        instance = this;
    }

    public string BuildText(string format) {
        var result = format;
        for (int i = 0; i < 100; i++) {
            var next = result;

            next = next.Replace("#{lastUnlockedLevel}", GameManager.game.AvailableLevelsInUnlockOrder().Count == 0 ? "" : GameManager.game.AvailableLevelsInUnlockOrder().Last().name);
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