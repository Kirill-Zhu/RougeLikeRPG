using MyStateMachine;
using UnityEngine;

public class PausedState : BaseState {
    public PausedState(SimpleCahracterController controller, Animator animator, HeroBattleController battleController, HeroAutoSkillController heroAutoSkillController) : base(controller, animator, battleController, heroAutoSkillController) {
    }
    public override void OnEnter() {
        base.OnEnter();
        heroBattleController.SkillDurationTimer = 0.5f;
    }
   
}