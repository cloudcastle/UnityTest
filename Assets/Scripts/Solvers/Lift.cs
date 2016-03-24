﻿using System.Collections;
using System.Linq;

namespace Solver
{
    public class Lift
    {
        public static int count = 0;

        public string name;

        public Lift() {
            ++count;
            this.name = string.Format("Lift #{0}", count);
        }

        public Lift(string name) {
            this.name = name;
        }

        public override string ToString() {
            return name;
        }

        public Lift Named(string name) {
            this.name = name;
            return this;
        }

        public Lift CallFrom(params Location[] locations) {
            locations.ToList().ForEach(location => {
                location.buttons.Add(new Button(this));
            });
            return this;
        }
    }
}