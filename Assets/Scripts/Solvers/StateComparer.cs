using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public class StateComparer : IEqualityComparer<State>
    {
        public bool Equals(State x, State y) {
            return x.calledLifts.SequenceEqual(y.calledLifts) && x.position == y.position;
        }

        public int GetHashCode(State state) {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + state.position.GetHashCode();
                state.calledLifts.ForEach(lift => {
                    hash = hash * 23 + lift.GetHashCode();
                });
                return hash;
            }
        }
    }
}