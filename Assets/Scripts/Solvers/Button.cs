using System.Collections;
using System.Collections.Generic;

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
            result.Achieve(state, new Push(this));
            return result;
        }

        public override string ToString() {
            return string.Format("button for {0}", target);
        }
    }
}