
using MyStateMachine;
using UnityEngine;

public class Locomotion : BaseState {
    public Locomotion(SimpleCahracterController controller, Animator animator, HeroBattleController heroBattleController) : base(controller, animator, heroBattleController) {}

    public override void OnEnter() {
        base.OnEnter();
        controller.RefreshJumpTimer();
        animator.CrossFade(Locomotion, duration);
    }

    public override void OnExit() {

    }

    public override void OnFixedUpdate() {

    }

    public override void OnUpdate() {
        controller.HandleMovement();
    }
}
