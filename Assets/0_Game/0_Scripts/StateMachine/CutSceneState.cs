using MyStateMachine;
using UnityEngine;

public class CutSceneState : BaseState {
    InGameUIManager inGameUIManager;
    public CutSceneState(SimpleCahracterController controller, Animator animator, HeroBattleController battleController, HeroAutoSkillController heroAutoSkillController, InGameUIManager inGameUIManager) : base(controller, animator, battleController, heroAutoSkillController) {
        this.inGameUIManager = inGameUIManager;
    }

    public override void OnEnter() {
        inGameUIManager.HideAllUI();
    }

    public override void OnExit() {
        inGameUIManager.ShowAllUI();
    }

    public override void OnFixedUpdate() {
        base.OnFixedUpdate();
    }

    public override void OnUpdate() {
        controller.HandleMovement();
    }
}
