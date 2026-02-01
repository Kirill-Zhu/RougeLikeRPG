namespace MyStateMachine {
    public class Transition : ITransition {
        public IState To { get; set; }
        public IPredicate condition { get; set; }

        public Transition(IState To, IPredicate condition) {
            this.To = To;
            this.condition = condition;
        }
    }
}