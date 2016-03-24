using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Solver
{
    public class Solution
    {
        public Puzzle puzzle;
        public State final;

        public HashSet<State> reachedStates;
        public HashSet<State> statesExitReachedFrom;

        public Solution(Puzzle puzzle, State final, HashSet<State> reachedStates, HashSet<State> statesExitReachedFrom = null) {
            this.puzzle = puzzle;
            this.final = final;
            this.reachedStates = reachedStates;
            this.statesExitReachedFrom = statesExitReachedFrom ?? new HashSet<State>();
        }

        public List<Action> Actions() {
            var result = new List<Action>();
            State current = final;
            while (current.previous != null) {
                result.Add(current.actionFromPrevious);
                current = current.previous;
            }
            result.Reverse();
            return result;
        }

        public override string ToString() {
            if (final == null) {
                return String.Format("Solution for {0} not found ({1} states reached, exit reached from {2} states)\n", puzzle, reachedStates.Count, statesExitReachedFrom.Count);
            }
            return String.Format(
                "Solution for {0} ({2} actions, {3} moves, {4} calls):\n{1}\n", 
                puzzle, 
                Actions().ExtToString("\n"), 
                Actions().Count, 
                Actions().Where(a => a is Move).Count(),
                Actions().Where(a => a is Push).Count()
            );
        }
    }
}