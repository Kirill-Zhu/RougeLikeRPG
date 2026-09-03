using MyStateMachine;
using UnityEngine;


namespace BossEntity {
    public abstract class BaseState : IState {
        protected readonly Hero hero;
        protected readonly Animator animator;
        protected readonly MoveController moveController;
        protected readonly BattleController battleController;
        protected readonly int locomotionHash = Animator.StringToHash("Locomotion");

        public BaseState(Hero hero, Animator animator, MoveController controller, BattleController battleController) {
            this.hero = hero;
            this.animator = animator;
            this.moveController = controller;
            this.battleController = battleController;
        }
        public virtual void OnEnter() {
            Debug.Log($" Boss is on state {GetType().Name}");
        }

        public virtual void OnExit() {

        }

        public virtual void OnFixedUpdate() {

        }

        public virtual void OnUpdate() {

        }
    }
}