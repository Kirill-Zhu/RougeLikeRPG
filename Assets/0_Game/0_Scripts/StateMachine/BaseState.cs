using UnityEngine;

namespace MyStateMachine {
    public abstract class BaseState : IState {

        protected readonly SimpleCahracterController controller;
        protected readonly Animator animator;
        protected readonly HeroBattleController heroBattleController;
        protected readonly float duration = 0.1f;
        public readonly int Locomotion = Animator.StringToHash("Locomotion");
        public readonly int Jump = Animator.StringToHash("Jump");
        public readonly int MeleAttack1 = Animator.StringToHash("Attack1");
        public readonly int MeleAttack2 = Animator.StringToHash("Attack2");
        public BaseState(SimpleCahracterController controller, Animator animator, HeroBattleController battleController) {
            this.controller = controller;
            this.animator = animator;
            this.heroBattleController = battleController;
        }
        public virtual void OnEnter() {
           // Debug.Log($"Enter : {this.GetType().Name} state");
        }

        public virtual void OnExit() {
           // Debug.Log($"Exit : {this.GetType().Name} state");
        }

        public virtual void OnFixedUpdate() {

        }

        public virtual void OnUpdate() {

        }
    }
}
