using UnityEngine;
using MyStateMachine;

public class Hero : MonoBehaviour
{
    SimpleCahracterController moveController;
    HeroBattleController battleContorller;
    StateMachine stateMachine = new StateMachine();
    [SerializeField] Animator animator;
    Locomotion locomotion;
    JumpState jumpState;
    LandingState landingState;
    SkillState skillState;
    private void Awake() {
        moveController = GetComponent<SimpleCahracterController>(); 
        battleContorller = GetComponent<HeroBattleController>();

        //StateMachine
        locomotion = new Locomotion(moveController, animator, battleContorller);
        jumpState = new JumpState(moveController, animator, battleContorller);
        landingState = new LandingState(moveController, animator,battleContorller);
        skillState = new SkillState(moveController, animator, battleContorller);

        //Movement
        At(locomotion, jumpState, new FuncPredicate(() => moveController.IsJumping));
        At(landingState, locomotion, new FuncPredicate(() => !moveController.IsJumping && moveController.Grounded()));
        
        //Skills
        At(locomotion, skillState, new FuncPredicate(() => moveController.Grounded() && battleContorller.IsUsingSkill));
        
        Any(locomotion, new FuncPredicate(() => !moveController.IsJumping && moveController.Grounded() && !battleContorller.IsUsingSkill));
        Any(landingState ,new FuncPredicate(() => !moveController.IsJumping && !moveController.Grounded()));
        stateMachine.SetState(locomotion);
    }

    private void Update() {
        stateMachine.Update();
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) =>stateMachine.AddAny(to, condition);
}
