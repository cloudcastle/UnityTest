using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class Level
{
    public string name;

    [NonSerialized]
    public List<Level> dependencies = new List<Level>();

    public Level(string name, params Level[] depends) {
        this.name = name;
        dependencies = depends.ToList();
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