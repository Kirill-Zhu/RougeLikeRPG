using MyStateMachine;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SafeZoneState : BaseState {
    PointerController pointerController;
    public SafeZoneState(SimpleCahracterController controller, Animator animator, HeroBattleController battleController, HeroAutoSkillController heroAutoSkillController, PointerController pointerController) : base(controller, animator, battleController, heroAutoSkillController) {
       this.pointerController = pointerController;
    }

    public override void OnEnter() {
        pointerController.EnablePointer();
    }

    public override void OnExit() {
        pointerController.DisablePointer();
    }

    public override void OnFixedUpdate() {
        pointerController.HandlePointToTarget();
    }

    public override void OnUpdate() {
        controller.HandleMovement();
        //Animations
        animator.SetFloat("MoveSpeed", controller.InputDirection.sqrMagnitude);
    }
}
