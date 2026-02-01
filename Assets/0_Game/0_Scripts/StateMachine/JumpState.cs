
using MyStateMachine;
using UnityEngine;

public class JumpState : BaseState {
    public JumpState(SimpleCahracterController controller, Animator animator, HeroBattleController heroBattleController) : base(controller, animator, heroBattleController) { }

    public override void OnEnter() {
        base.OnEnter();
    }

    public override void OnExit() {

    }

    public override void OnFixedUpdate() {

    }

    public override void OnUpdate() {
        controller.HandleJumping();
    }
}