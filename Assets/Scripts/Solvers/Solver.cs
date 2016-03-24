using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public class Solver
    {
        public static Solution Solve(Puzzle puzzle) {
            HashSet<State> reachedStates = new HashSet<State>(new StateComparer());
            State start = StartState(puzzle);
            Bfs(puzzle, reachedStates, start);
            State finish = reachedStates.FirstOrDefault(Final);
            return new Solution(puzzle, finish);
        }

        public static bool Final(State state) {
            return state.position.isExit;
        }

        public static void Dfs(Puzzle puzzle, HashSet<State> reachedStates, State start) {
            if (reachedStates.Contains(start)) {
                return;
            }
            reachedStates.Add(start);
            List<State> nextStates = NextStates(puzzle, start);
            nextStates.ForEach(nextState => {
                Dfs(puzzle, reachedStates, nextState);
            });
        }

        public static void Bfs(Puzzle puzzle, HashSet<State> reachedStates, State start) {
            Queue<State> queue = new Queue<State>();
            queue.Enqueue(start);
            reachedStates.Add(start);
            while (queue.Count > 0) {
                List<State> nextStates = NextStates(puzzle, queue.Dequeue());
                nextStates.ForEach(nextState => {
                    if (!reachedStates.Contains(nextState)) {
                        reachedStates.Add(nextState);
                        queue.Enqueue(nextState);
                    }
                });
            }
        }

        public static List<State> NextStates(Puzzle puzzle, State state) {
            var result = new List<State>();
            state.position.edgesFrom.ForEach(edge => {
                var nextState = edge.Move(state);
                if (nextState != null) {
                    result.Add(nextState);
                }
            }); 
            state.position.buttons.ForEach(button => {
                var nextState = button.Push(state);
                if (nextState != null) {
                    result.Add(nextState);
                }
            });
            return result;
        }

        public static State StartState(Puzzle puzzle) {
            return new State(puzzle.start);
        }
    }
}