using System.Collections.Generic;
using System;

[Serializable]
public class Game
{
    public readonly List<Level> levels;

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

        var ascention = new Level("Ascention", depends: precaution);
     
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
        };
    }
}