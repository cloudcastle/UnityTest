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
        var test1 = new Level("Test 1");
        var test2 = new Level("Test 2");
        var test3 = new Level("Test 3");
        var test4 = new Level("Test 4");
        var test5 = new Level("Test 5");
        var test6 = new Level("Test 6");
     
        levels = new List<Level>() {
            click,
            mouseMove,
            wasd,
            space,
            raise,
            order,

            test1,
            test2,
            test3,
            test4,
            test5,
            test6,
        };
    }
}