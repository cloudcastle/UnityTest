using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solver;

namespace Terminal
{
    class Program
    {
        static void Main(string[] args) {
            PrintAllSolutions();
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        static void PrintAllSolutions() {
            Console.WriteLine(Puzzles.Game().Select(Solver.Solver.Solve).ToList().ExtToString("\n"));
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
