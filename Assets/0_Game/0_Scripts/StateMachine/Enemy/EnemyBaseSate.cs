using MyStateMachine;
using System;
using UnityEngine;

namespace Enemies.MyStateMachine {
    public abstract class EnemyBaseSate : IState {

        protected readonly Animator animator;
        protected Func<float> OnVelocityChange;
        protected readonly float duration = 0.1f;
        protected readonly int locomotionHash = Animator.StringToHash("Locomotion");
        protected readonly int battleHash = Animator.StringToHash("Battle");
        public EnemyBaseSate(Animator animator, Func<float> velocityEvent) { this.animator = animator; this.OnVelocityChange = velocityEvent; }
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnExit() { }

    }
}

