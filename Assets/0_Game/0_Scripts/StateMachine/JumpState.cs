
using MyStateMachine;
using UnityEngine;

public class JumpState : BaseState {
    public JumpState(SimpleCahracterController controller, Animator animator, HeroBattleController heroBattleController, HeroAutoSkillController heroAutoSkillController) : base(controller, animator, heroBattleController, heroAutoSkillController) { }

    public override void OnEnter() {
        base.OnEnter();
    }

    public override void OnExit() {

    }

    public override void OnFixedUpdate() {

    }

    public override void OnUpdate() {
        controller.HandleJumping();

        //AutoBattle
        heroAutoSkillController.OnUpdate();
    }
}