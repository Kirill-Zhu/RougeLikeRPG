using UnityEngine;


namespace BossEntity {
    public class Locomotion : BaseState {
        public Locomotion(Hero hero, Animator animator, MoveController moveController, BattleController battleController) : base(hero, animator, moveController, battleController) {
        }

        float velocity = 0;
        public override void OnEnter() {
            base.OnEnter();
            velocity = 0;
            animator.CrossFade(locomotionHash, 0.1f);
        }

        public override void OnExit() {

        }

        public override void OnFixedUpdate() {

        }

        public override void OnUpdate() {
            moveController.OnUpdate();
            velocity = Mathf.Lerp(velocity, moveController.Velocity, Time.deltaTime * 4);
            animator.SetFloat("Velocity", velocity);
        }
    }
}