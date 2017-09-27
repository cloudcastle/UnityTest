#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable declared but not used.

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

    public GameSettings settings = new GameSettings();
     
    public Level currentLevel;

    public Vector2 levelGraphCameraPosition = Vector2.zero;
    public float levelGraphCameraZoom = 1;

    public Game() {
        current = this;

        // Intro
        var click = new Level(0, "Click");
        var mouseMove = new Level(0, "MouseMove", depends: click);
        var wasd = new Level(0, "WASD", depends: mouseMove);
        var fall = new Level(0, "Fall", wasd);
        var space = new Level(0, "Space", depends: fall);

        // Lifts Intro
        var raise = new Level(0, "Raise", depends: wasd);
        var shift = new Level(0, "Shift", depends: raise);

        // Jumps Intro Part 1
        var gap = new Level(1, "Gap", shift, space);
        var spring = new Level(1, "Spring", gap);

        // Moving Surfaces Intro
        var ride = new Level(1, "Ride", spring);
        var f = new Level(1, "F", ride);

        // Controls
        var t = new Level(1, "T", ride, f);
        var splitSecond = new Level(1, "Split second", t);
        var z = new Level(2, "Z", splitSecond);

        // Jumps Intro Part 2
        var string_ = new Level(1, "String", z);
        var cornerCase = new Level(2, "Corner case", string_);

        // Lifts       
        var order = new Level(2, "Order", string_);
        var precaution = new Level(3, "Precaution", order);
        var stairs = new Level(4, "Stairs", precaution, f);
        var twoPits = new Level(4, "Two pits", precaution, f);
        var hold = new Level(4, "Hold", precaution, f);
        // Lifts II
        var ascention = new Level(5, "Ascention", hold);
        var launch = new Level(5, "Launch", hold);
        var temple = new Level(5, "Temple", hold);
        var underground = new Level(5, "Underground", hold);
        var tower = new Level(5, "Tower", hold);

        // Lifts Tricks
        var rhythm = new Level(3, "Rhythm", order, shift);
        var secondBeat = new Level(3, "Second beat", rhythm);
        var trio = new Level(4, "Trio", secondBeat);

        // Moving Surfaces
        var fly = new Level(2, "Fly", string_);
        var phase = new Level(2, "Phase", string_);
        var malapropos = new Level(3, "Malapropos", phase);
        var meetInTheMiddle = new Level(2, "Meet in the middle", fly, raise);

        // Jumps
        var climb = new Level(2, "Climb", gap, z, string_);
        var sixBoxes = new Level(3, "Six boxes", climb);
        var snake = new Level(4, "Snake", spring, sixBoxes);

        // Buttons intro
        var open = new Level(0, "Open", wasd);
        var close = new Level(0, "Close", open, string_);

        // Keys intro
        var pass = new Level(0, "Pass", wasd);
        var fit = new Level(0, "Fit", pass);
        var cleanHands = new Level(0, "Clean hands", fit);
        var repick = new Level(1, "Repick", cleanHands);
        var rightMouseButton = new Level(2, "Right mouse button", repick, open);
        var walkthrough = new Level(1, "Walkthrough", rightMouseButton);
        var qeMouseWheel = new Level(2, "QE mouse wheel", walkthrough, close);

        // Keys
        var greenCabin = new Level(3, "Green cabin", qeMouseWheel);
        var blueDoor = new Level(3, "Blue door", greenCabin);
        var merge = new Level(3, "Merge", greenCabin);
        var push = new Level(3, "Push", merge);
        var blueCabin = new Level(3, "Blue cabin", greenCabin);
        var redCabin = new Level(3, "Red cabin", greenCabin);
        var stretch = new Level(3, "Stretch", greenCabin);
        var triple = new Level(3, "Triple", greenCabin, redCabin, blueCabin);
        var pair = new Level(3, "Pair", triple);
        var keySequence = new Level(3, "Key sequence", triple);
        var keyChain = new Level(3, "Key chain", triple);

        // Jumps tricks
        var workaround = new Level(2, "Workaround", gap, z, close);

        // Keys Tricks
        var column = new Level(1, "Column", qeMouseWheel);
        var bridge = new Level(2, "Bridge", string_, z, qeMouseWheel);
        var stand = new Level(1, "Stand", bridge);
        var reach = new Level(1, "Reach", bridge);
        var slit = new Level(3, "Slit", reach);
        var grab = new Level(2, "Grab", slit);
        var forgotten = new Level(2, "Forgotten", stand, grab);
        var floor = new Level(2, "Floor", grab);
        var barehanded = new Level(4, "Barehanded", floor);
        var pool = new Level(3, "Pool", bridge);
        var tool = new Level(1, "Tool", column, stand);
        var intersection = new Level(3, "Intersection", tool);
        var interception = new Level(4, "Interception", intersection/*timestop*/);
        var accuracy = new Level(2, "Accuracy", stand, tool);
        var v = new Level(1, "V", reach, workaround);

        // Keys Anticolors
        var ban = new Level(1, "Ban", qeMouseWheel);
        var dorBlue = new Level(3, "Dor Blue", ban, blueDoor);

        // Tricks
        var bars = new Level(2, "Bars", string_);
        var memories = new Level(4, "Memories", z, bars);

        // Twins Intro
        var you = new Level(2, "You", pass);
        var gift = new Level(1, "Gift", you, walkthrough);
        var giveALift = new Level(2, "Give a lift", you, raise);
        var bat = new Level(1, "Bat", giveALift, gap);
        var together = new Level(2, "Together", bat, z);
        var fat = new Level(2, "Fat", together);
        var place = new Level(3, "Place", fat, gift);

        // Twins
        var traverse = new Level(3, "Traverse", place);
        var pisa = new Level(2, "Pisa", z, string_, column);
        var deal = new Level(4, "Deal", place, column);
        var catch_ = new Level(3, "Catch", deal);
        var coveredHill = new Level(4, "Covered Hill", forgotten, deal);

        // Mixed
        var delivery = new Level(2, "Delivery", rightMouseButton, raise, qeMouseWheel);



        // Timestop
        var juggle = new Level(2, "Juggle", t, string_, delivery);

        // Twins tricks
        var bacon = new Level(3, "Bacon", fat);
        var apart = new Level(2, "Apart", bacon);
        var doubleDive = new Level(2, "Double dive", apart);
        var farewell = new Level(2, "Farewell", doubleDive);
        var invite = new Level(3, "Invite", farewell, traverse);
        var williamTell = new Level(4, "William Tell", deal, open);
        var notWilliamTell = new Level(4, "Not William Tell", williamTell, open);

        // Slowmo
        var doubleJump = new Level(3, "Double jump", doubleDive, v);
        var skywalker = new Level(3, "Skywalker", v);
        var toss = new Level(3, "Toss", bat, v);
        var longDoubleJump = new Level(3, "Long double jump", doubleJump);
        var extendedJump = new Level(3, "Extended Jump", longDoubleJump, toss);
        var duck = new Level(3, "Duck", doubleJump);
        var upkeep = new Level(3, "Upkeep", apart, v);
        var link = new Level(3, "Link", upkeep);
        var boo = new Level(3, "Boo", slit, apart, v);

        // Portals
        var edge = new Level(2, "Edge", pass);
        var serve = new Level(1, "Serve", edge);
        var levitation = new Level(2, "Levitation", stand);
        var up = new Level(4, "Up", levitation);
        var forward = new Level(4, "Forward", levitation);
        var support = new Level(3, "Support", levitation);
        var flip = new Level(3, "Flip", levitation);
        var collect = new Level(3, "Collect", flip);
        var exchange = new Level(3, "Exchange", flip);

        // Timestop again
        var claws = new Level(3, "Claws", support);
        var _2cor11_33 = new Level(4, "2 Cor 11 33", support, gift, greenCabin);
        var stop = new Level(5, "Stop", support, close);

        // Buttons 
        var closer = new Level(3, "Closer", close, giveALift, v);
        var tin = new Level(3, "Tin", walkthrough, close);
        var telekinesis = new Level(3, "Telekinesis", tin, flip);

        // Gates
        var gate = new Level(1, "Gate", serve);
        var unpack = new Level(2, "Unpack", gate);
        var trip = new Level(2, "Trip", unpack);
        var extract = new Level(2, "Extract", trip);
        var parcel = new Level(2, "Parcel", trip);
        var spiral = new Level(3, "Spiral", forgotten, trip);

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