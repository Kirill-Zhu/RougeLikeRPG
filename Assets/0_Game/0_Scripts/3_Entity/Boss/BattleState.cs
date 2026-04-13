using UnityEngine;


namespace BossEntity {
    public class BattleState : BaseState {
        public BattleState(Hero hero, Animator animator, MoveController controller, BattleController battleController) : base(hero, animator, controller, battleController) {
        }

        public override void OnEnter() {
            base.OnEnter();
            battleController.OnAnimation += HandleAnimationsAndDirection;
        }

        public override void OnExit() {
            battleController.OnAnimation -= HandleAnimationsAndDirection;
        }

        public override void OnFixedUpdate() {

        }

        public override void OnUpdate() {
            battleController.OnUpdate();
        }

        private void HandleAnimationsAndDirection(int animationHash, float animationDuration = 1) {
            animator.StopPlayback();

            if (animationDuration > 0) {
                var clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
                float aniamtionSpeed = clip.length / animationDuration;
                animator.speed = aniamtionSpeed;
                animator.Play(animationHash);
            } else {
                animator.speed = 1;
            }
            //controller.HandleInFightMovement();//Attack In Move Direction 
        }
    }
}