using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
public class Level
{
    public string name;

    public List<Level> dependencies = new List<Level>();

    [NonSerialized]
    public HashSet<Level> transitiveDependencies;

    public Level(string name, params Level[] depends) {
        this.name = name;
        dependencies = depends.ToList();
        if (Game.current != null)
        {
            if (Game.current.levels.Any(level => level.name == name)) {
                Debug.LogError(string.Format("Duplicate level name: {0}", name));
            }
            Game.current.levels.Add(this);
        }
    }

    public bool Unlocked() {
        return dependencies.All(level => level.Completed());
    }

    public bool Completed() {
        return GameManager.game.completedLevels.Contains(this);
    }

    public void ToggleCompleted() {
        if (Completed()) {
            GameManager.game.completedLevels.Remove(this);
        } else {
            GameManager.game.completedLevels.Add(this);
        }
        GameManager.instance.Save();
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

    public void PrecalculateTransitiveDependencies() {
        dependencies.ForEach(l => l.PrecalculateTransitiveDependencies());
        transitiveDependencies = new HashSet<Level>();
        dependencies.ForEach(l => {
            l.transitiveDependencies.ToList().ForEach(d => transitiveDependencies.Add(d));
            transitiveDependencies.Add(l);
        });
    }

    public bool Depends(Level level) {
        PrecalculateTransitiveDependencies();
        return transitiveDependencies.Contains(level);
    }

    public override string ToString() {
        return name;
    }
}