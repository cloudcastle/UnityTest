using System.Collections;
using System.Collections.Generic;
using System;

namespace Solver
{
    public class Solution
    {
        public Puzzle puzzle;
        public State final;

        public Solution(Puzzle puzzle, State final) {
            this.puzzle = puzzle;
            this.final = final;
        }

        public List<Action> Actions() {
            var result = new List<Action>();
            State current = final;
            while (current.parent != null) {
                result.Add(current.action);
                current = current.parent;
            }
            result.Reverse();
            return result;
        }

        public override string ToString() {
            if (final == null) {
                return String.Format("Solution for {0} not found\n", puzzle);
            }
            return String.Format("Solution for {0} ({2} actions):\n{1}\n", puzzle, Actions().ExtToString("\n"), Actions().Count);
        }
    }
}