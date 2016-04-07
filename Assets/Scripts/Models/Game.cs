﻿using System.Collections.Generic;
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
        var space = new Level("Space", depends: wasd);

        // Lifts Intro
        var raise = new Level("Raise", depends: wasd);
        var shift = new Level("Shift", depends: raise);
        var order = new Level("Order", depends: shift);
        var precaution = new Level("Precaution", depends: order);
        var f = new Level("F", depends: shift);


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

        // Jumps Intro
        var gap = new Level("Gap", shift, space);
        var spring = new Level("Spring", gap);
        var fall = new Level("Fall", wasd);

        // Moving Surfaces
        var ride = new Level("Ride", space);
        var fly = new Level("Ride", ride);
        var phase = new Level("Ride", ride);

        // Controls
        var r = new Level("R", fall);
        var z = new Level("Z", r, ride);

        // Jumps
        var sixBoxes = new Level("Six boxes", gap);
        var climb = new Level("Climb", gap);
        var snake = new Level("Snake", spring);

        // Keys intro
        var pass = new Level("Pass", wasd);
        var fit = new Level("Fit", pass);
        var cleanHands = new Level("Clean hands", fit);
        var repick = new Level("Repick", cleanHands);
        var rightMouseButton = new Level("Right mouse button", repick);
        var qeMouseWheel = new Level("QE mouse wheel", rightMouseButton);

        // Keys
        var blueDoor = new Level("Blue door", qeMouseWheel);
        var blueCabin = new Level("Blue cabin", qeMouseWheel);
        var pair = new Level("Pair", qeMouseWheel);

        currentLevel = click;

        if (!CheckGameCorrectness()) {
            Debug.LogError("Game is incorrect!");
        }
    }

    private bool CheckGameCorrectness() {
        return levels.All(level => level.dependencies.All(dependency => levels.Contains(dependency)));
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