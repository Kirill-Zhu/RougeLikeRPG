
using System;

namespace MyStateMachine {
    public class FuncPredicate : IPredicate {

        readonly Func<bool> condition;

        public FuncPredicate(Func<bool> condition) {
            this.condition = condition;
        }

        public bool Evaluate() {
            return condition.Invoke();
        }
    }
}