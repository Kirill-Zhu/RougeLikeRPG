using MyStateMachine;
using UnityEngine;
using Zenject;


namespace BossEntity {
    [RequireComponent(typeof(BattleController), typeof(MoveController))]
    public class Boss : MonoBehaviour {
        public string Name;
        public string Description;
        public Texture Label;
        //Properties
        Hero hero;
        Animator animator;
        BattleController battleController;
        MoveController moveController;
        IBrain brain;
        public bool inBattle;
        //State machine
        StateMachine stateMachine = new StateMachine();
        Locomotion locomotion;
        BattleState battleState;
        public void Initialize(Hero hero) {
            this.hero = hero;
            //Get 
            animator = GetComponent<Animator>();
            battleController = GetComponent<BattleController>();
            moveController = GetComponent<MoveController>();
            brain = new Brain(this, battleController, moveController);
            //Initialize
            moveController.Initialize(hero.transform);
            battleController.Initialize(hero.transform);
            //State machine
            locomotion = new Locomotion(hero, animator, moveController, battleController);
            battleState = new BattleState(hero, animator, moveController, battleController);

            At(locomotion, battleState, new FuncPredicate(() => inBattle));
            At(battleState, locomotion, new FuncPredicate(() => !inBattle));

            stateMachine.SetState(locomotion);
        }
        private void FixedUpdate() {
            brain.UpdateDecision();
            stateMachine.FixedUpdate();

        }
        private void Update() {
            stateMachine.Update();
            brain.OnUpdate();
        }
        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAny(to, condition);

        public class Brain : IBrain {
            readonly Boss boss;
            readonly BattleController battleController;
            readonly MoveController moveController;
            public Brain(Boss boss, BattleController battleController, MoveController moveController) {
                this.boss = boss;
                this.battleController = battleController;
                this.moveController = moveController;
            }
            public void OnUpdate() {
                battleController.UpdateCoolDowns();
            }
            public void UpdateDecision() {
                if (battleController.animationDuration > 0) {
                    boss.inBattle = true;
                    return;
                }
               boss.inBattle = battleController.Evaluate(moveController.DistanceToHero);
            }
        }
    }
    public interface IBrain {
        public void UpdateDecision() { }
        public void OnUpdate();
    }
}