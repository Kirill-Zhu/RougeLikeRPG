using MyStateMachine;
using UnityEngine;

public class SkillState : BaseState {
    public SkillState(SimpleCahracterController controller, Animator animator, HeroBattleController heroBattleController, HeroAutoSkillController heroAutoSkillController) : base(controller, animator, heroBattleController, heroAutoSkillController) {
    }

    public override void OnEnter() {
        base.OnEnter();
        heroBattleController.SubscribeInputs();

        heroBattleController.OnAnimationStart += HandleAnimationsAndDirection;
        heroBattleController.UseSkill(heroBattleController.FitstInputIndex);                //Use skill was approved for the first time
    }

    public override void OnExit() {
        base.OnExit();
        heroBattleController.UnSubscribeInputs();
        animator.StopPlayback();
        heroBattleController.OnAnimationStart -= HandleAnimationsAndDirection;
    }

    public override void OnFixedUpdate() {

    }

    public override void OnUpdate() {
        heroBattleController.HandleUpdateStatus();
        heroAutoSkillController.OnUpdate();
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
            controller.HandleInFightMovement();//Attack In Move Direction 
    }
}
