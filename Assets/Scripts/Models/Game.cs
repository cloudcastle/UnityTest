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
        var click = new Level("Click");
        var mouseMove = new Level("MouseMove", depends: click);
        var wasd = new Level("WASD", depends: mouseMove);
        var fall = new Level("Fall", wasd);
        var space = new Level("Space", depends: fall);

        // Lifts Intro
        var raise = new Level("Raise", depends: wasd);
        var shift = new Level("Shift", depends: raise);

        // Jumps Intro Part 1
        var gap = new Level("Gap", shift, space);
        var spring = new Level("Spring", gap);

        // Moving Surfaces Intro
        var ride = new Level("Ride", spring);
        var f = new Level("F", ride);

        // Controls
        var t = new Level("T", ride, f);
        var z = new Level("Z", t);

        // Jumps Intro Part 2
        var string_ = new Level("String", z);
        var cornerCase = new Level("Corner case", string_);

        // Lifts       
        var order = new Level("Order", string_);
        var precaution = new Level("Precaution", order);
        var stairs = new Level("Stairs", precaution, f);
        var twoPits = new Level("Two pits", precaution, f);
        var hold = new Level("Hold", precaution, f);
        // Lifts II
        var ascention = new Level("Ascention", hold);
        var launch = new Level("Launch", hold);
        var temple = new Level("Temple", hold);
        var underground = new Level("Underground", hold);
        var tower = new Level("Tower", hold);

        // Lifts Tricks
        var rhythm = new Level("Rhythm", order, shift);
        var secondBeat = new Level("Second beat", rhythm);
        var trio = new Level("Trio", secondBeat);

        // Moving Surfaces
        var fly = new Level("Fly", string_);
        var phase = new Level("Phase", string_);
        var malapropos = new Level("Malapropos", phase);
        var meetInTheMiddle = new Level("Meet in the middle", fly, raise);

        // Jumps
        var climb = new Level("Climb", gap, z, string_);
        var sixBoxes = new Level("Six boxes", climb);
        var snake = new Level("Snake", spring, sixBoxes);

        // Buttons intro
        var open = new Level("Open", wasd, string_);
        var close = new Level("Close", open);

        // Keys intro
        var pass = new Level("Pass", wasd);
        var fit = new Level("Fit", pass);
        var cleanHands = new Level("Clean hands", fit);
        var repick = new Level("Repick", cleanHands);
        var rightMouseButton = new Level("Right mouse button", repick);
        var walkthrough = new Level("Walkthrough", rightMouseButton, open);
        var qeMouseWheel = new Level("QE mouse wheel", walkthrough);

        // Keys
        var greenCabin = new Level("Green cabin", qeMouseWheel);
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
        var column = new Level("Column", qeMouseWheel);
        var bridge = new Level("Bridge", string_, z, qeMouseWheel);
        var stand = new Level("Stand", bridge);
        var reach = new Level("Reach", bridge);
        var slit = new Level("Slit", reach);
        var grab = new Level("Grab", slit);
        var forgotten = new Level("Forgotten", stand, grab);
        var floor = new Level("Floor", grab);
        var barehanded = new Level("Barehanded", floor);
        var pool = new Level("Pool", bridge);
        var tool = new Level("Tool", column, stand);
        var intersection = new Level("Intersection", tool);
        var interception = new Level("Interception", intersection/*timestop*/);
        var accuracy = new Level("Accuracy", stand, tool);
        var v = new Level("V", reach);

        // Keys Anticolors
        var ban = new Level("Ban", qeMouseWheel);
        var dorBlue = new Level("Dor Blue", ban, blueDoor);

        // Tricks
        var bars = new Level("Bars", string_);
        var memories = new Level("Memories", z, bars);

        // Twins Intro
        var you = new Level("You", pass);
        var gift = new Level("Gift", you, walkthrough);
        var giveALift = new Level("Give a lift", you, raise);
        var bat = new Level("Bat", giveALift, gap);
        var together = new Level("Together", bat, z);
        var fat = new Level("Fat", together);
        var place = new Level("Place", fat, gift);

        // Twins
        var traverse = new Level("Traverse", place);
        var pisa = new Level("Pisa", z, place, string_);
        var deal = new Level("Deal", place, column);
        var catch_ = new Level("Catch", deal);
        var coveredHill = new Level("Covered Hill", forgotten, deal);

        // Mixed
        var delivery = new Level("Delivery", qeMouseWheel);


        // Jumps tricks
        var workaround = new Level("Workaround", open);

        // Timestop
        var splitSecond = new Level("Split second", z);
        var juggle = new Level("Juggle", t, delivery);

        // Twins tricks
        var bacon = new Level("Bacon", fat, splitSecond);
        var apart = new Level("Apart", bacon);
        var doubleDive = new Level("Double dive", apart);
        var farewell = new Level("Farewell", doubleDive);
        var invite = new Level("Invite", farewell, traverse);
        var williamTell = new Level("William Tell", deal, open);
        var notWilliamTell = new Level("Not William Tell", williamTell, open);

        // Slowmo
        var doubleJump = new Level("Double jump", doubleDive, v);
        var skywalker = new Level("Skywalker", v);
        var toss = new Level("Toss", bat, v);
        var longDoubleJump = new Level("Long double jump", doubleJump);
        var extendedJump = new Level("Extended Jump", longDoubleJump, toss);
        var duck = new Level("Duck", doubleJump);
        var upkeep = new Level("Upkeep", apart);
        var link = new Level("Link", upkeep);
        var boo = new Level("Boo", slit, apart);

        // Portals
        var edge = new Level("Edge", fit);
        var serve = new Level("Serve", edge, tool);
        var levitation = new Level("Levitation", bacon, serve);
        var up = new Level("Up", levitation);
        var forward = new Level("Forward", levitation);
        var support = new Level("Support", levitation);
        var flip = new Level("Flip", levitation);
        var collect = new Level("Collect", flip);
        var exchange = new Level("Exchange", flip);

        // Timestop again
        var claws = new Level("Claws", support);
        var _2cor11_33 = new Level("2 Cor 11 33", support, gift, greenCabin);
        var stop = new Level("Stop", support, close);

        // Buttons 
        var closer = new Level("Closer", close, giveALift, v);
        var tin = new Level("Tin", walkthrough, close);
        var telekinesis = new Level("Telekinesis", tin, flip);

        // Gates
        var gate = new Level("Gate", serve);
        var unpack = new Level("Unpack", gate);
        var trip = new Level("Trip", unpack);
        var extract = new Level("Extract", trip);
        var parcel = new Level("Parcel", trip);
        var spiral = new Level("Spiral", forgotten, trip);

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