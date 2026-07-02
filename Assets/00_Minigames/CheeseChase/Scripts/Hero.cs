using MyStateMachine;
using UnityEngine;

namespace CheeseChase {

    public class Hero : MonoBehaviour {
       
        HeroMoveController characterController;
        Animator animator;

        //State Machine
        StateMachine stateMachine;
        LocomotionState locomotion;
        IdleState idleState;
        private void Awake() {
            characterController = GetComponent<HeroMoveController>();  
            animator = GetComponent<Animator>();

            //State Machine
            stateMachine = new StateMachine();
            locomotion = new LocomotionState(characterController,animator);
            idleState = new IdleState(characterController, animator);

            //At(idleState, locomotion, new FuncPredicate(()=> )
            
        }


        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAny(to, condition);
    }


}
