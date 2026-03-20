using System;
using UnityEngine;

namespace Enemies.MyStateMachine {
    public class EnemyLocomotionState : EnemyBaseSate {
        public EnemyLocomotionState(Animator animator, Func<float> velocityEvent) : base(animator, velocityEvent) {
        }

        public override void OnEnter() {
            animator.CrossFade(locomotionHash, duration);
        }

        public override void OnExit() {

        }

        public override void OnFixedUpdate() {
            animator.SetFloat("Velocity", OnVelocityChange.Invoke());
        }

        public override void OnUpdate() {

        }
    }
}

