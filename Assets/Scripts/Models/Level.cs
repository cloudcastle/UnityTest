﻿using System.Collections.Generic;
using System.Linq;

public class Level
{
    public bool completed = false;

    public readonly string name;

    public readonly List<Level> dependencies = new List<Level>();

    public Level(string name, params Level[] depends) {
        this.name = name;
        dependencies = depends.ToList();
    }

    public bool Unlocked() {
        return dependencies.All(level => level.completed);
    }
}