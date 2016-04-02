using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class Game
{
    public readonly List<Level> levels;

    public List<Level> completedLevels = new List<Level>();

    public Settings settings = new Settings();

    public Level currentLevel;

    public Game() {
        var click = new Level("Click");
        var mouseMove = new Level("MouseMove", depends: click);
        var wasd = new Level("WASD", depends: mouseMove);
        var space = new Level("Space", depends: wasd);

        var raise = new Level("Raise", depends: wasd);
        var shift = new Level("Shift", depends: raise);
        var order = new Level("Order", depends: shift);
        var precaution = new Level("Precaution", depends: order);

        var stairs = new Level("Stairs", depends: precaution);
        var twoPits = new Level("Two pits", depends: precaution);
        var hold = new Level("Hold", depends: precaution);

        var ascention = new Level("Ascention", depends: hold);
        var launch = new Level("Launch", depends: hold);
        var temple = new Level("Temple", depends: hold);
        var underground = new Level("Underground", depends: hold);
        var tower = new Level("Tower", depends: hold);

        var gap = new Level("Gap", shift, space);
        var spring = new Level("Spring", gap);

        var sixBoxes = new Level("Six boxes", gap);
        var climb = new Level("Climb", gap);
        var snake = new Level("Snake", spring);

        var pass = new Level("Pass", wasd);
        var fit = new Level("Fit", pass);
        var cleanHands = new Level("Clean hands", fit);
        var repick = new Level("Repick", cleanHands);
        var rightMouseButton = new Level("Right mouse button", repick);
        var qeMouseWheel = new Level("QE mouse wheel", rightMouseButton);

        var blueDoor = new Level("Blue door", qeMouseWheel);
        var blueCabin = new Level("Blue cabin", qeMouseWheel);

     
        levels = new List<Level>() {
            // Intro
            click,
            mouseMove,
            wasd,
            space,

            // Lift Intro
            raise,
            order,
            precaution,

            // Lift
            stairs,
            twoPits,
            hold,
            ascention,
            launch,
            temple,
            underground,
            tower,

            // Jump Intro
            shift,
            gap,
            spring,

            // Jump
            sixBoxes,
            climb,
            snake,

            // Key Intro
            pass,
            fit,
            cleanHands,
            repick,
            rightMouseButton,
            qeMouseWheel,

            // Key
            blueDoor,
            blueCabin,
        };

        currentLevel = click;
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