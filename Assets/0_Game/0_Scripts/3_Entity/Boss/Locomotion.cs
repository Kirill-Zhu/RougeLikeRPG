using UnityEngine;


namespace BossEntity {
    public class Locomotion : BaseState {
        public Locomotion(Hero hero, Animator animator, MoveController controller, BattleController battleController) : base(hero, animator, controller, battleController) {
        }

        public override void OnEnter() {
            base.OnEnter();
            animator.CrossFade(locomotionHash, 0.1f);
        }

        public override void OnExit() {

        }

        public override void OnFixedUpdate() {

        }

        public override void OnUpdate() {
            moveController.OnUpdate();
        }
    }
}