using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
public class Level
{
    public string name;

    public List<Level> dependencies = new List<Level>();

    public Level(string name, params Level[] depends) {
        this.name = name;
        dependencies = depends.ToList();
        if (Game.current.levels.Any(level => level.name == name)) {
            Debug.LogError(string.Format("Duplicate level name: {0}", name));
        }
        Game.current.levels.Add(this);
    }

    public bool Unlocked() {
        return dependencies.All(level => level.Completed());
    }

    public bool Completed() {
        return GameManager.game.completedLevels.Contains(this);
    }

    public int CompletionOrder() {
        return GameManager.game.completedLevels.IndexOf(this);
    }

    public int UnlockOrder() {
        return dependencies.ExtMax(level => level.CompletionOrder());
    }

    public int GameOrder() {
        return GameManager.game.levels.IndexOf(this);
    }

    public override string ToString() {
        return name;
    }
}