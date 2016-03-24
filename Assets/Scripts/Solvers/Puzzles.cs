using System.Collections;
using System.Collections.Generic;

namespace Solver
{
    public static class Puzzles
    {
        public static Puzzle Raise() {
            var low = new Location("Low");
            var high = new Location("High").WithExit();
            var lift = low.LiftTo(high).Named("L");
            low.buttons.Add(new Button(lift));
            return new Puzzle("Raise", low, high);
        }

        public static Puzzle Order() {
            var loc0 = new Location("0");
            var loc1 = new Location("1");
            var loc2 = new Location("2").WithExit();
            var lift1 = loc0.LiftTo(loc1).Named("L1");
            var lift2 = loc1.LiftTo(loc2).Named("L2");
            loc0.buttons.Add(new Button(lift1));
            loc0.buttons.Add(new Button(lift2));
            return new Puzzle("Order", loc0, loc1, loc2);
        }

        public static Puzzle Precaution() {
            var loc0 = new Location("0");
            var loc1 = new Location("1");
            var loc2 = new Location("2").WithExit();
            var high = new Location("High");
            var lift1 = loc0.LiftTo(loc1).Named("L1").CallFrom(loc0);
            var lift2 = loc1.LiftTo(loc2).Named("L2").CallFrom(high);
            var liftToHigh = loc0.LiftTo(high).Named("H").CallFrom(loc1);
            return new Puzzle("Precaution", loc0, loc1, loc2, high);
        }

        public static Puzzle Stairs() {
            var loc0 = new Location("0");
            var loc1 = new Location("1");
            var loc2 = new Location("2");
            var loc3 = new Location("3");
            var exit = new Location("exit").WithExit();
            var a1 = loc0.DirectLiftTo(loc1).Named("A1");
            var b1 = loc0.DirectLiftTo(loc1).Named("B1");
            var a2 = loc1.DirectLiftTo(loc2).Named("A2");
            var b2 = loc1.DirectLiftTo(loc2).Named("B2");
            var a3 = loc2.DirectLiftTo(loc3).Named("A3");
            var b3 = loc2.DirectLiftTo(loc3).Named("B3");
            var e = loc3.LiftTo(exit).CallFrom(loc0).Named("E");
            return new Puzzle("Stairs", loc0, loc1, loc2, loc3, exit);
        }

        public static Puzzle TwoPits() {
            var result = new Puzzle("Two Pits");

            var ground = new Location("ground");
            var pit1 = new Location("pit 1");
            var pit2 = new Location("pit 2");
            var floor1 = new Location("floor 1");
            var floor2 = new Location("floor 2").WithExit();

            var l1 = ground.LiftTo(floor1).Named("L1").CallFrom(pit2);
            var l2 = floor1.LiftTo(floor2).Named("L2").CallFrom(pit1);
            var pit1self = pit1.DirectLiftTo(ground).Named("pit 1 self");
            var pit2self = pit2.DirectLiftTo(ground).Named("pit 2 self");
            var pit1other = pit1.LiftTo(ground).Named("pit 1 other").CallFrom(pit2);
            var pit2other = pit2.LiftTo(ground).Named("pit 2 other").CallFrom(pit1);

            return result;
        }

        public static List<Puzzle> Game() {
            return new List<Puzzle>()
            {
                Raise(),
                Order(),
                Precaution(),
                Stairs(),
                TwoPits()
            };
        }
    }
}