using System;
using UnityEngine;

namespace Enemies.MyStateMachine {
    public class EnemyBattleState : EnemyBaseSate {

        readonly SimpleEnemyBattleContorller battleContoller;
        public EnemyBattleState(Animator animator, Func<float> velocityEvent, SimpleEnemyBattleContorller battleContoller) : base(animator, velocityEvent) {
            this.battleContoller = battleContoller;
        }

        public override void OnEnter() {
            //animator.CrossFade(battleHash, duration);
            battleContoller.TryAttack();
            battleContoller.OnAnimationStart += OnAnimation;
        }

        public override void OnExit() {
            battleContoller.OnAnimationStart -= OnAnimation;
            battleContoller.ExitBalltle(); //Cts cancelling UniTasks
        }

        public override void OnFixedUpdate() {
            battleContoller.OnFixedUpdate();
            animator.SetFloat("Velocity", OnVelocityChange.Invoke());
        }

        public override void OnUpdate() {
          
        }
        void OnAnimation(int animationHash) {
            animator.CrossFade(animationHash, duration);    
            animator.Play(animationHash, -1, 0f);
            //Debug.Log($"{GetType().Name} plays OnAnimation ");
        }
    }
}

