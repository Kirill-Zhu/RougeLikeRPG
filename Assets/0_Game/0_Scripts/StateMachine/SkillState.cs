using MyStateMachine;
using UnityEngine;

public class SkillState : BaseState {
    public SkillState(SimpleCahracterController controller, Animator animator, HeroBattleController heroBattleController) : base(controller, animator, heroBattleController) {
    }

    public override void OnEnter() {
        base.OnEnter();
        heroBattleController.SubscribeInputs();
        heroBattleController.UseSkill(heroBattleController.FitstInputIndex);

    }

    public override void OnExit() {
        base.OnExit();
        heroBattleController.UnSubscribeInputs();
    }

    public override void OnFixedUpdate() {

    }

    public override void OnUpdate() {
        heroBattleController.HandleUpdateStatus();
    }
}