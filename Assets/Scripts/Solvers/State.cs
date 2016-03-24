using System.Collections;
using System.Collections.Generic;

namespace Solver
{
    public class State
    {
        public Location position;

        public List<Lift> calledLifts = new List<Lift>();

        public State previous;
        public Action actionFromPrevious;
        public State next;
        public Action actionToNext;

        public State() {
        }

        public State(Location position, List<Lift> calledLifts = null) {
            this.position = position;
            this.calledLifts = calledLifts ?? new List<Lift>();
        }

        public State Clone() {
            return new State(position, new List<Lift>(calledLifts));
        }

        public void SetPrevious(State previous, Action actionFromPrevious) {
            this.previous = previous;
            this.actionFromPrevious = actionFromPrevious;
        }

        public void SetNext(State next, Action actionToNext) {
            this.next = next;
            this.actionToNext = actionToNext;
        }

        public override string ToString() {
            return string.Format("At {0} with queue {1}", position, calledLifts.ExtToString());
        }
    }
}