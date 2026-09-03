using MyStateMachine;
using UnityEngine;

namespace CheeseChase {

    public abstract class BaseState : IState {

        HeroMoveController controller;

        //Animations
        Animator animator;
        int locomotion = Animator.StringToHash("Locomotion");
        int idleHash = Animator.StringToHash("Idle");

        public BaseState(HeroMoveController contorller, Animator animator) {
            this.animator = animator;
        }
        public virtual void OnEnter() {

        }

        public virtual void OnExit() {

        }

        public virtual void OnFixedUpdate() {

        }

        public virtual void OnUpdate() {

        }
    }

    public class LocomotionState : BaseState {
        
        public LocomotionState(HeroMoveController contorller, Animator animator) : base(contorller, animator) {
        }

        public override void OnEnter() {
            base.OnEnter();
        }

        public override void OnExit() {
            base.OnExit();
        }

        public override void OnFixedUpdate() {
            base.OnFixedUpdate();
        }

        public override void OnUpdate() {
            base.OnUpdate();
        }
    }
    public class IdleState : BaseState {
        public IdleState(HeroMoveController contorller, Animator animator) : base(contorller, animator) {
        }

        public override void OnEnter() {
            base.OnEnter();
        }

        public override void OnExit() {
            base.OnExit();
        }

        public override void OnFixedUpdate() {
            base.OnFixedUpdate();
        }

        public override void OnUpdate() {
            base.OnUpdate();
        }
    }
}

