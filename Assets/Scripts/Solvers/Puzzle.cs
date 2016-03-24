using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public class Puzzle
    {
        public static Puzzle current;

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
    }
}