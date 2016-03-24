using System.Collections;
using System.Collections.Generic;

namespace Solver
{
    public class Push : Action
    {
        public Button target;

        public Push(Button target) {
            this.target = target;
        }

        public override string ToString() {
            return string.Format("call {0}", target.target);
        }
    }
}