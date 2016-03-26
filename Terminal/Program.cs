using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solver;
using System.IO;

namespace Terminal
{
    class Program
    {
        static void Main(string[] args) {
            var writer = new StreamWriter(new FileStream("output.txt", FileMode.Create, FileAccess.Write));
            Console.SetOut(writer);
            Work();
            writer.Close();
        }

        static void Work() {
            PrintSolution(Puzzles.Temple());
            //Console.WriteLine("HW");
        }

        private static void Play() {
            var floor1 = "floor 1";
            var floor2 = "floor 2";
            var floor3 = "floor 3";
            var floor4 = "floor 4";
            var ground = "ground";
            
            Puzzle x = Puzzles.Ascention();
            Solution s = Solver.Solver.Solve(x);
            x.Start();
            x.Call("1");
            x.Move(floor1);
            x.Call("2");
            x.Move(ground);
            x.Move(floor2);
            x.Call("3");
            x.Move(ground);
            x.Move(floor3);
            x.Call("4");
            x.Move(floor2);
            x.Call("3");
            x.Move(ground);
            x.Move(floor4);
            x.Move(floor3);
            x.Call("4");
            x.Move(floor2);
            x.Move(floor1);
            x.Call("2");
            x.Move(ground);
            x.Move(floor3);
            x.Move(floor2);
            x.Call("3");
            x.Move(ground);
            x.Move(floor4);
            x.Move(floor3);
            x.Call("4");


            Console.WriteLine(String.Format("Currest state: {0}\nThis state visited: {1}", x.state, s.reachedStates.Contains(x.state)));
        }

        static void PrintSolution(Puzzle puzzle) {
            Console.WriteLine(Solver.Solver.Solve(puzzle));
        }

        static void PrintAllSolutions() {
            List<Solution> solutions = Puzzles.Game().Select(Solver.Solver.Solve).ToList();
            if (solutions.Any(s => s.final == null)) {
                throw new Exception("Unsolvable level!");
            }
            Console.WriteLine(solutions.ExtToString("\n"));
        }

        static void TestComparer() {
            Puzzle puzzle = Puzzles.Raise();
            State state1 = new State(puzzle.start, new List<Lift>() { puzzle.locations[0].edgesFrom[0].lift });
            State state2 = new State(puzzle.start, new List<Lift>() { puzzle.locations[0].edgesFrom[0].lift });
            StateComparer comparer = new StateComparer();
            Console.WriteLine(comparer.Equals(state1, state2));
            Console.WriteLine(comparer.GetHashCode(state1));
            Console.WriteLine(comparer.GetHashCode(state2));
            Console.ReadLine();
        }

        static void TestNextStates() {
            Puzzle puzzle = Puzzles.Raise();
            State state1 = new State(puzzle.start, new List<Lift>() { puzzle.locations[0].edgesFrom[0].lift });
            State state2 = new State(puzzle.start, new List<Lift>() { puzzle.locations[0].edgesFrom[0].lift });
            StateComparer comparer = new StateComparer();
            Console.WriteLine(comparer.Equals(state1, state2));
            Console.WriteLine(comparer.GetHashCode(state1));
            Console.WriteLine(comparer.GetHashCode(state2));
            Console.ReadLine();
        }
    }
}
