#pragma warning disable 0168 // variable declared but not used.

using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Game
{
    public static Game current;

    public readonly List<Level> levels = new List<Level>();

    public List<Level> completedLevels = new List<Level>();

    public Settings settings = new Settings();

    public Level currentLevel;

    public Game() {
        current = this;

        // Intro
        var click = new Level("Click");
        var mouseMove = new Level("MouseMove", depends: click);
        var wasd = new Level("WASD", depends: mouseMove);
        var escape = new Level("Escape", depends: wasd);
        var space = new Level("Space", depends: wasd);

        // Moving Surfaces Intro
        var ride = new Level("Ride", space);
        var f = new Level("F", depends: ride);

        // Lifts Intro
        var raise = new Level("Raise", depends: wasd);
        var shift = new Level("Shift", depends: raise);
        var order = new Level("Order", depends: shift);
        var precaution = new Level("Precaution", depends: order);

        // Lifts
        var stairs = new Level("Stairs", depends: precaution);
        var twoPits = new Level("Two pits", depends: precaution);
        var hold = new Level("Hold", depends: precaution);
        // Lifts II
        var ascention = new Level("Ascention", depends: hold);
        var launch = new Level("Launch", depends: hold);
        var temple = new Level("Temple", depends: hold);
        var underground = new Level("Underground", depends: hold);
        var tower = new Level("Tower", depends: hold);

        // Lifts Tricks
        var rhythm = new Level("Rhythm", order, shift);
        var secondBeat = new Level("Second Beat", rhythm);
        var trio = new Level("Trio", secondBeat);

        // Jumps Intro
        var gap = new Level("Gap", shift, space);
        var spring = new Level("Spring", gap);
        var fall = new Level("Fall", wasd);

        // Controls
        var r = new Level("R", fall);
        var z = new Level("Z", r, ride);

        // Moving Surfaces
        var fly = new Level("Fly", z);
        var phase = new Level("Phase", z);
        var malapropos = new Level("Malapropos", phase);

        // Jumps
        var sixBoxes = new Level("Six boxes", gap, z);
        var climb = new Level("Climb", gap, z);
        var snake = new Level("Snake", spring);
        var string_ = new Level("String", r);

        // Keys intro
        var pass = new Level("Pass", wasd);
        var fit = new Level("Fit", pass);
        var cleanHands = new Level("Clean hands", fit);
        var repick = new Level("Repick", cleanHands);
        var rightMouseButton = new Level("Right mouse button", repick);
        var qeMouseWheel = new Level("QE mouse wheel", rightMouseButton, r);

        // Keys
        var greenCabin = new Level("Green Cabin", qeMouseWheel);
        var blueDoor = new Level("Blue door", greenCabin);
        var merge = new Level("Merge", greenCabin);
        var push = new Level("Push", merge);
        var blueCabin = new Level("Blue cabin", greenCabin);
        var redCabin = new Level("Red cabin", greenCabin);
        var stretch = new Level("Stretch", greenCabin);
        var triple = new Level("Triple", greenCabin, redCabin, blueCabin);
        var pair = new Level("Pair", triple);
        var keySequence = new Level("Key sequence", triple);
        var keyChain = new Level("Key chain", triple);

        // Keys Tricks
        var walkthrough = new Level("Walkthrough", cleanHands);
        var column = new Level("Column", walkthrough, z);
        var bridge = new Level("Bridge", string_, r, pass);
        var pool = new Level("Pool", bridge);

        // Tricks
        var bars = new Level("Bars", wasd);

        currentLevel = click;
        current = null;

        ClearDependencies();
    }

    private void ClearDependencies() {
        while (true) {
            bool changed = false;
            levels.ForEach(level => {
                var excessDependency = level.dependencies.FirstOrDefault(dependency => {
                    var dependenciesExceptCurrent = level.dependencies.ShallowClone();
                    dependenciesExceptCurrent.Remove(dependency);
                    var transitiveDependencies = Algorithms.Reachable<Level>(level, l => l == level ? dependenciesExceptCurrent : l.dependencies);
                    return transitiveDependencies.Contains(dependency);
                });
                if (excessDependency != null) {
                    level.dependencies.Remove(excessDependency);
                    changed = true;
                }
            });
            if (!changed) {
                break;
            }
        }
    }

    public List<Level> AvailableLevelsInUnlockOrder() {
        var result = GameManager.game.levels.Where(level => level.Unlocked() && !level.Completed()).ToList();
        result.Sort(new LevelComparerByUnlock());
        return result;
    }

    public List<Level> AvailableLevelsInReverseUnlockOrder() {
        var result = AvailableLevelsInUnlockOrder();
        result.Reverse();
        return result;
    }
}