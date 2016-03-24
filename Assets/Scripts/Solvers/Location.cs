using System.Collections;
using System.Collections.Generic;

namespace Solver
{
    public class Location
    {
        public static int count;

        public string name;

        public List<Button> buttons = new List<Button>();

        public bool isExit;

        public List<Edge> edgesFrom = new List<Edge>();
        public List<Edge> edgesTo = new List<Edge>();

        public Lift LiftTo(Location target) {
            Lift lift = new Lift();
            edgesFrom.Add(new Edge(this, target, lift));
            target.edgesFrom.Add(new Edge(target, this));
            return lift;
        }

        public Lift DirectLiftTo(Location target) {
            Lift lift = new Lift().CallFrom(this);
            edgesFrom.Add(new Edge(this, target, lift));
            target.edgesFrom.Add(new Edge(target, this));
            return lift;
        }

        public Location() : this(string.Format("Location #{0}", ++count)) {
        }

        public Location(string name) {
            this.name = name;
            if (Puzzle.current != null) {
                Puzzle.current.locations.Add(this);
                if (Puzzle.current.locations.Count == 1) {
                    Puzzle.current.start = Puzzle.current.locations[0];
                }
            }
        }

        public override string ToString() {
            return name;
        }

        public Location WithExit() {
            this.isExit = true;
            return this;
        }

        public void JumpTo(Location target) {
            edgesFrom.Add(new Edge(this, target));
        }
    }
}