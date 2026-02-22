using Enemies.MyStateMachine;
using MyStateMachine;
using UnityEngine;

namespace Enemies {

    [RequireComponent(typeof(Animator), typeof(Rigidbody))]
    public abstract class Enemy : Entity {
    
        public Animator animator;
        protected const float crossaFadeDuration = 0.1f;
        //Battle
      

        //State Machine
        protected  StateMachine stateMachine = new StateMachine();
        protected EnemyLocomotionState locomotion;
        protected EnemyBattleState battle;
        

        protected override void Awake() {
            base.Awake();
            animator = GetComponent<Animator>();
          

            //State Machine
            locomotion = new EnemyLocomotionState(animator, () => VelocityMagnitude);
            battle = new EnemyBattleState(animator, () => VelocityMagnitude, battleContorller);

            //State Conditions
            At(locomotion, battle, new FuncPredicate(() => InBattle));
            At(battle, locomotion, new FuncPredicate(() => !InBattle));

            stateMachine.SetState(locomotion);

            //Animation 
            battleContorller.OnAnimationStart += naimationHash => animator.CrossFade(naimationHash, crossaFadeDuration);

        }
        private void Update() {
            stateMachine?.Update();
        }
        private void FixedUpdate() {
            stateMachine?.FixedUpdate();
        }
        public override void Die() {
            base.Die();
            animator.CrossFade("Die", crossaFadeDuration);
            stateMachine = null;
        }
        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAny(to, condition);

    }
}
