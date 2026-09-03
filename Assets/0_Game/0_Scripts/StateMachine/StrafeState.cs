using MyStateMachine;
using UnityEngine;

public class StrafeState :BaseState, IState {
    HeroRendererController rendererController;
    public StrafeState(SimpleCahracterController controller, Animator animator, HeroBattleController battleController, HeroAutoSkillController heroAutoSkillController, HeroRendererController rendererController) : base(controller, animator, battleController, heroAutoSkillController) {
     this.rendererController = rendererController;
    }

    public void OnEnter() {
        base.OnEnter();
        rendererController.SetStrafeHeroMareial();
        controller.HandleStrafe();
        this.controller.gameObject.layer = LayerMask.NameToLayer("IgnoreEnemies");
    }

    public void OnExit() {
        controller.RefreshStrafeCooldownTimer();
        controller.RefreshStrafeTimer();
        rendererController.SetStandartHeroMaterial();
        this.controller.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void OnFixedUpdate() {
        
    }

    public void OnUpdate() {
        controller.HandleStrafe();
    }
}
