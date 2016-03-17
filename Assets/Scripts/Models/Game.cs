using System.Collections.Generic;

public class Game
{
    public readonly List<Level> levels;

    public Game() {
        var click = new Level("Click");
        var mouseMove = new Level("MouseMove", depends: click);
        var wasd = new Level("WASD", depends: mouseMove);
        var test1 = new Level("Test 1", depends: click);
        var test2 = new Level("Test 2", depends: click);
        var test3 = new Level("Test 3", depends: click);
        var test4 = new Level("Test 4", depends: click);
     
        levels = new List<Level>() {
            click,
            mouseMove,
            wasd,

            test1,
            test2,
            test3,
            test4,
        };
    }
}