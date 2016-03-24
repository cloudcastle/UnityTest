using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Solver
{
    public class Puzzle
    {
        public static Puzzle current;

        public State state;

        public string name;

        public List<Location> locations = new List<Location>();

        public Location start;

        public Puzzle() {
        }

        public Puzzle(string name, params Location[] locations) {
            this.name = name;
            if (locations.Count() != 0) {
                this.locations = locations.ToList();
                start = locations[0];
            } else {
                current = this;
            }
        }

        public override string ToString() {
            return name;
        }

        public void Start() {
            state = new State(start);
        }

        public void Call(string liftName) {
            var button = state.position.buttons.FirstOrDefault(b => b.target.name == liftName);
            if (button == null) {
                throw new Exception(String.Format("Call {0} failed: no button for such lift at current position {1}", liftName, state.position));
            }
            if (state.calledLifts.Contains(button.target)) {
                throw new Exception(String.Format("Call {0} failed: already in queue", liftName));
            }
            state = button.Push(state);
        }

        public void Move(string locationName, string liftName = null) {
            Edge edge = state.position.edgesFrom.FirstOrDefault(e => e.to.name == locationName && e.Move(state) != null && (liftName == null || e.lift != null && e.lift.name == liftName));
            if (edge == null) {
                throw new Exception(String.Format("Move to {0}{1} failed: no suitable edge", locationName, liftName != null ? string.Format(" via {0)", liftName) : ""));
            }
            state = edge.Move(state);
        }
    }
}