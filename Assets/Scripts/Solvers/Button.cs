using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public class Button
    {
        public Lift target;

        public Button(Lift lift) {
            this.target = lift;
        }

        public State Push(State state) {
            if (state.calledLifts.Contains(target)) {
                return null;
            }
            var result = state.Clone();
            result.calledLifts.Add(target);
            result.SetPrevious(state, new Push(this));
            return result;
        }

        public State Unpush(State state) {
            if (state.calledLifts.Count == 0 || state.calledLifts.Last() != this.target) {
                return null;
            }
            var result = state.Clone();
            result.calledLifts.RemoveAt(result.calledLifts.Count - 1);
            result.SetNext(state, new Push(this));
            return result;
        }

        public override string ToString() {
            return string.Format("button for {0}", target);
        }
    }
}