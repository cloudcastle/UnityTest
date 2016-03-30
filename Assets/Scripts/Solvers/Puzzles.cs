#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

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
            var floor2 = new Location("floor 2");
            var floor3 = new Location("floor 3");
            var floor4 = new Location("floor 4").WithExit();

            var l1 = ground.LiftTo(floor1).Named("L1").CallFrom(pit2);
            var l2 = floor1.LiftTo(floor2).Named("L2").CallFrom(pit1);
            var l3 = floor2.LiftTo(floor3).Named("L3").CallFrom(pit2);
            var l4 = floor3.LiftTo(floor4).Named("L4").CallFrom(pit1);

            var pit1self = pit1.DirectLiftTo(ground).Named("pit 1 self");
            var pit2self = pit2.DirectLiftTo(ground).Named("pit 2 self");

            var pit1other = pit1.LiftTo(ground).Named("pit 1 other").CallFrom(pit2);
            var pit2other = pit2.LiftTo(ground).Named("pit 2 other").CallFrom(pit1);

            return result;
        }

        public static Puzzle Hold() {
            var result = new Puzzle("Hold");

            var ground = new Location("ground");
            var pit = new Location("pit");  
            var floor1 = new Location("floor 1");
            var floor2 = new Location("floor 2").WithExit();
            var tower1 = new Location("tower 1");
            var tower2 = new Location("tower 2");
            var tower3 = new Location("tower 3");

            pit.DirectLiftTo(floor1).Named("launch").CallFrom(pit);
            ground.LiftTo(floor1).Named("hold").CallFrom(floor1);
            floor1.LiftTo(floor2).Named("final").CallFrom(tower3);
            ground.DirectLiftTo(tower1).Named("tower 1 lift");
            ground.LiftTo(tower2).Named("tower 2 lift").CallFrom(tower1);
            ground.LiftTo(tower3).Named("tower 3 lift").CallFrom(tower2);
            ground.JumpTo(pit);

            return result;
        }

        public static Puzzle Ascention() {
            var result = new Puzzle("Ascention");

            var ground = new Location("ground");
            var floor1 = new Location("floor 1");
            var floor2 = new Location("floor 2");
            var floor3 = new Location("floor 3");
            var floor4 = new Location("floor 4");
            var exitFloor1 = new Location("exit floor 1");
            var exitFloor2 = new Location("exit floor 2");
            var exitFloor3 = new Location("exit floor 3");
            var exitFloor4 = new Location("exit floor 4").WithExit();

            ground.LiftTo(floor1).Named("1").CallFrom(ground);
            ground.LiftTo(floor2).Named("2").CallFrom(floor1);
            ground.LiftTo(floor3).Named("3").CallFrom(floor2);
            ground.LiftTo(floor4).Named("4").CallFrom(floor3);

            floor4.JumpTo(floor3);
            floor3.JumpTo(floor2);
            floor2.JumpTo(floor1);
            floor1.JumpTo(ground);

            exitFloor4.JumpTo(ground);
            exitFloor3.JumpTo(ground);
            exitFloor2.JumpTo(ground);
            exitFloor1.JumpTo(ground);

            ground.LiftTo(exitFloor1).Named("E1").CallFrom(floor1);
            exitFloor1.LiftTo(exitFloor2).Named("E2").CallFrom(floor2);
            exitFloor2.LiftTo(exitFloor3).Named("E3").CallFrom(floor3);
            exitFloor3.LiftTo(exitFloor4).Named("E4").CallFrom(floor4);
            
            return result;
        }

        public static Puzzle Launch() {
            var result = new Puzzle("Launch");

            var ground = new Location("ground");
            var tower1 = new Location("tower 1");
            var tower2 = new Location("tower 2");
            var tower3 = new Location("tower 3");
            var tower4 = new Location("tower 4");
            var exitFloor1 = new Location("exit floor 1");
            var exitFloor2 = new Location("exit floor 2");
            var exitFloor3 = new Location("exit floor 3");
            var exitFloor4 = new Location("exit floor 4").WithExit();
            var top = new Location("top");

            ground.DirectLiftTo(top).Named("main");

            ground.LiftTo(tower1).Named("1").CallFrom(tower2);
            ground.LiftTo(tower2).Named("2").CallFrom(tower1, tower3);
            ground.LiftTo(tower3).Named("3").CallFrom(tower2, tower4);
            ground.LiftTo(tower4).Named("4").CallFrom(tower3);

            exitFloor4.JumpTo(ground);
            exitFloor3.JumpTo(ground);
            exitFloor2.JumpTo(ground);
            exitFloor1.JumpTo(ground);

            top.JumpTo(tower1);
            top.JumpTo(tower2);
            top.JumpTo(tower3);
            top.JumpTo(tower4);

            tower4.JumpTo(ground);
            tower3.JumpTo(ground);
            tower2.JumpTo(ground);
            tower1.JumpTo(ground);

            ground.LiftTo(exitFloor1).Named("E1").CallFrom(tower1);
            exitFloor1.LiftTo(exitFloor2).Named("E2").CallFrom(tower2);
            exitFloor2.LiftTo(exitFloor3).Named("E3").CallFrom(tower3);
            exitFloor3.LiftTo(exitFloor4).Named("E4").CallFrom(tower4);
            return result;
        }

        public static Puzzle LaunchModified() {
            var result = new Puzzle("Launch");

            var ground = new Location("ground");
            var tower1 = new Location("tower 1");
            var tower2 = new Location("tower 2");
            var tower3 = new Location("tower 3");
            var tower4 = new Location("tower 4");
            var exitFloor1 = new Location("exit floor 1");
            var exitFloor2 = new Location("exit floor 2");
            var exitFloor3 = new Location("exit floor 3");
            var exitFloor4 = new Location("exit floor 4");
            var exitFloor5 = new Location("exit floor 5").WithExit();
            var top = new Location("top");

            ground.DirectLiftTo(top).Named("main");

            ground.LiftTo(tower1).Named("1").CallFrom(tower2);
            ground.LiftTo(tower2).Named("2").CallFrom(tower1, tower3);
            ground.LiftTo(tower3).Named("3").CallFrom(tower2, tower4);
            ground.LiftTo(tower4).Named("4").CallFrom(tower3);

            exitFloor4.JumpTo(ground);
            exitFloor3.JumpTo(ground);
            exitFloor2.JumpTo(ground);
            exitFloor1.JumpTo(ground);

            top.JumpTo(tower1);
            top.JumpTo(tower2);
            top.JumpTo(tower3);
            top.JumpTo(tower4);

            top.JumpTo(ground);

            tower4.JumpTo(ground);
            tower3.JumpTo(ground);
            tower2.JumpTo(ground);
            tower1.JumpTo(ground);

            ground.LiftTo(exitFloor1).Named("E1").CallFrom(tower1);
            exitFloor1.LiftTo(exitFloor2).Named("E2").CallFrom(tower2);
            exitFloor2.LiftTo(exitFloor3).Named("E3").CallFrom(tower3);
            exitFloor3.LiftTo(exitFloor4).Named("E4").CallFrom(tower4);
            exitFloor4.LiftTo(exitFloor5).Named("E5").CallFrom(tower1);
            return result;
        }

        public static Puzzle Temple() {
            var result = new Puzzle("Temple");

            var ground = new Location("ground");
            var floor1a = new Location("floor 1 A");
            var floor1b = new Location("floor 1 B");
            var floor2 = new Location("floor 2");
            var floor3 = new Location("floor 3");
            var highPit = new Location("high pit");
            var lowPit = new Location("low pit");
            var exit = new Location("exit").WithExit();

            lowPit.DirectLiftTo(floor3).Named("low pit lift").CallFrom(floor1b);
            floor3.LiftTo(exit).Named("exit lift").CallFrom(highPit);
            highPit.DirectLiftTo(floor2).Named("high pit self").CallFrom(floor1a);
            ground.LiftTo(floor1b).Named("floor 1 B lift").CallFrom(floor2);
            floor1b.LiftTo(floor2).Named("floor 1 B - floor 2 lift").CallFrom(highPit);

            floor3.JumpTo(floor2);
            floor2.JumpTo(floor1a);
            floor1a.JumpTo(ground);
            ground.JumpTo(lowPit);

            return result;
        }

        public static Puzzle Quarters() {
            var result = new Puzzle("Quarters");

            var zone1 = new Location("zone 1");
            var zone2 = new Location("zone 2");
            var zone3 = new Location("zone 3");
            var zone4 = new Location("zone 4");
            var exit = new Location("exit").WithExit();

            zone1.JumpLiftTo(zone2).Named("12A").CallFrom(zone1, zone2);
            zone1.JumpLiftTo(zone2).Named("12B").CallFrom(zone1, zone2);
            zone2.JumpLiftTo(zone3).Named("23A").CallFrom(zone2, zone3);
            zone2.JumpLiftTo(zone3).Named("23B").CallFrom(zone2, zone3);
            zone3.JumpLiftTo(zone4).Named("34A").CallFrom(zone3, zone4);
            zone3.JumpLiftTo(zone4).Named("34B").CallFrom(zone3, zone4);
            zone4.JumpLiftTo(zone1, back: false).Named("R").CallFrom(zone4);
            zone4.LiftTo(exit).Named("E").CallFrom(zone1);

            return result;
        }

        public static List<Puzzle> Game() {
            return new List<Puzzle>()
            {
                Raise(),
                Order(),
                Precaution(),
                Stairs(),
                TwoPits(),
                Hold(),
                Ascention(),
                Launch(),
            };
        }
    }
}