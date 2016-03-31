using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class Game
{
    public readonly List<Level> levels;

    public List<Level> completedLevels = new List<Level>();

    public Game() {
        var click = new Level("Click");
        var mouseMove = new Level("MouseMove", depends: click);
        var wasd = new Level("WASD", depends: mouseMove);
        var space = new Level("Space", depends: wasd);

        var raise = new Level("Raise", depends: wasd);
        var shift = new Level("Shift", depends: raise);
        var order = new Level("Order", depends: raise);
        var precaution = new Level("Precaution", order, shift);

        var stairs = new Level("Stairs", depends: precaution);
        var twoPits = new Level("Two pits", depends: precaution);
        var hold = new Level("Hold", depends: precaution);

        var ascention = new Level("Ascention", depends: hold);
        var launch = new Level("Launch", depends: hold);
        var temple = new Level("Temple", depends: hold);
        var underground = new Level("Underground", depends: hold);
        var tower = new Level("Tower", depends: hold);

        var gap = new Level("Gap", shift, space);

        var sixBoxes = new Level("Six boxes", gap);
        var climb = new Level("Climb", gap);

        var pass = new Level("Pass", wasd);
        var fit = new Level("Fit", pass);
        var cleanHands = new Level("Clean hands", fit);
        var repick = new Level("Repick", cleanHands);
        var rightMouseButton = new Level("Right mouse button", repick);
        var qeMouseWheel = new Level("QE mouse wheel", rightMouseButton);

        var blueDoor = new Level("Blue door", rightMouseButton);

     
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

            // Jump
            sixBoxes,
            climb,

            // Key Intro
            pass,
            fit,
            cleanHands,
            repick,
            rightMouseButton,
            qeMouseWheel,

            // Key
            blueDoor
        };
    }

    public List<Level> AvailableLevelsInUnlockOrder() {
        var result = GameManager.game.levels.Where(level => level.Unlocked() && !level.Completed()).ToList();
        result.Sort(new LevelComparerByUnlock());
        return result;
    }
}