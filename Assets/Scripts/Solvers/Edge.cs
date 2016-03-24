using System.Collections;

namespace Solver
{
    public class Edge
    {
        public Location from;
        public Location to;

        public Lift lift;

        public Edge(Location from, Location to, Lift lift = null) {
            this.from = from;
            this.lift = lift;
            this.to = to;
        }

        public State Move(State state) {
            if (this.lift != null && (state.calledLifts.Count == 0 || state.calledLifts[0] != this.lift)) {
                return null;
            }
            var result = state.Clone();
            result.position = to;
            if (this.lift != null) {
                result.calledLifts.RemoveAt(0);
            }
            result.SetPrevious(state, new Move(this));
            return result;
        }

        public State Unmove(State state) {
            if (this.lift != null && (state.calledLifts.Contains(this.lift))) {
                return null;
            }
            var result = state.Clone();
            result.position = from;
            if (this.lift != null) {
                result.calledLifts.Insert(0, this.lift);
            }
            result.SetNext(state, new Move(this));
            return result;
        }

        public override string ToString() {
            return string.Format("edge from {0} to {1}{2}", from, to, lift == null ? "" : string.Format(" via {2}", lift));
        }
    }
}