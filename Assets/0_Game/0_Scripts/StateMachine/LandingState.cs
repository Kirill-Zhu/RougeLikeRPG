
using MyStateMachine;
using UnityEngine;

public class LandingState : BaseState {
    public LandingState(SimpleCahracterController controller, Animator animator, HeroBattleController heroBattleController, HeroAutoSkillController heroAutoSkillController) : base(controller, animator, heroBattleController, heroAutoSkillController) {}

    public override void OnEnter() {
        base.OnEnter();
    }

    public override void OnExit() {

    }

    public override void OnFixedUpdate() {

    }

    public override void OnUpdate() {
        controller.HandleLanding();

        //AutoBattle
        heroAutoSkillController.OnUpdate();
    }
}
