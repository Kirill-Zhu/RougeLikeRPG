namespace MyStateMachine {
    public interface ITransition {
        public IState To { get; set; }

        public IPredicate condition { get; set; }


    }
}