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
        var order = new Level("Order", depends: raise);
        var precaution = new Level("Precaution", depends: order);

        var stairs = new Level("Stairs", depends: precaution);
        var twoPits = new Level("Two pits", depends: precaution);
        var hold = new Level("Hold", depends: precaution);

        var ascention = new Level("Ascention", depends: hold);
        var launch = new Level("Launch", depends: hold);
        var temple = new Level("Temple", depends: hold);
        var underground = new Level("Underground", depends: hold);
        var tower = new Level("Tower", depends: hold);

        var shift = new Level("Shift", depends: raise);
        var gap = new Level("Gap", shift, space);

        var sixBoxes = new Level("Six boxes", gap);

     
        levels = new List<Level>() {
            click,
            mouseMove,
            wasd,
            space,

            raise,
            order,
            precaution,

            stairs,
            twoPits,
            hold,

            ascention,
            launch,
            temple,
            underground,
            tower,

            shift,
            gap,
            sixBoxes
        };
    }

    public List<Level> AvailableLevelsInUnlockOrder() {
        var result = GameManager.game.levels.Where(level => level.Unlocked() && !level.Completed()).ToList();
        result.Sort(new LevelComparerByUnlock());
        return result;
    }
}