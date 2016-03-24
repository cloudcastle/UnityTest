using System.Collections;
using System.Collections.Generic;

namespace Solver
{
    public class State
    {
        public Location position;

        public List<Lift> calledLifts = new List<Lift>();

        public State parent;

        public Action action;

        public State() {
        }

        public State(Location position, List<Lift> calledLifts = null) {
            this.position = position;
            this.calledLifts = calledLifts ?? new List<Lift>();
        }

        public State Clone() {
            return new State(position, new List<Lift>(calledLifts));
        }

        public void Achieve(State parent, Action action) {
            this.parent = parent;
            this.action = action;
        }

        public override string ToString() {
            return string.Format("At {0} with queue {1}", position, calledLifts.ExtToString());
        }
    }
}