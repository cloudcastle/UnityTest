using System.Collections;
using System.Collections.Generic;
using System;

namespace Solver
{
    public class Move : Action
    {
        public Edge target;

        public Move(Edge target) {
            this.target = target;
        }

        public override string ToString() {
            if (target.lift == null) {
                return String.Format("jump down to {0}", target.to);
            }
            //return String.Format("move to {0}{1}", target.to, String.Format(" via {0}", target.lift));
            return String.Format("lift to {0}", target.to);
        }
    }
}