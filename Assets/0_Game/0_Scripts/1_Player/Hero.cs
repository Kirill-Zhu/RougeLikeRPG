using MyStateMachine;
using UnityEngine;
using UnityEngine.Events;

public class Hero : MonoBehaviour {

    //Initialize
    public UnityEvent OnHeroChange;                                                                                         ///Invokes Every time when need change UI
    public UnityEvent OnLevelUp;
    public UnityEvent OnChooseLelvelUpCard;
    HeroStrategyData heroData;

    SimpleCahracterController moveController;
    
    //Battle
    public HeroBattleController HeroBattleController => battleContorller;
    HeroBattleController battleContorller;
    
    //Health
    public HealthComponent HealthComponent => healthComponent;
    [SerializeField] HealtComponentData healthData;
    HealthComponent healthComponent;
    
    //Expiriance
    ExpComponent expComponent;
    
    //State Machine
    StateMachine stateMachine = new StateMachine();
    [SerializeField] Animator animator;
    Locomotion locomotion;
    JumpState jumpState;
    LandingState landingState;
    SkillState skillState;
    bool gameOnPause = false;

    public void Initialize(HeroStrategyData data) {
        heroData = data;

        //Health
        healthComponent.Initialize(heroData.HealtComponentData);
        
        //Battle
        battleContorller.Initialize(heroData.SkillStrategyData);
        
        //Exp
        expComponent.Initialize(OnLevelUp);

        OnHeroChange?.Invoke();
    }

    private void Awake() {
        moveController = GetComponent<SimpleCahracterController>();
        battleContorller = GetComponent<HeroBattleController>();
        healthComponent = GetComponent<HealthComponent>();
        expComponent = GetComponent<ExpComponent>();

        //StateMachine
        locomotion = new Locomotion(moveController, animator, battleContorller);
        jumpState = new JumpState(moveController, animator, battleContorller);
        landingState = new LandingState(moveController, animator, battleContorller);
        skillState = new SkillState(moveController, animator, battleContorller);

        //Movement
        At(locomotion, jumpState, new FuncPredicate(() => moveController.IsJumping));
        At(landingState, locomotion, new FuncPredicate(() => !moveController.IsJumping && moveController.Grounded()));
        At(skillState, jumpState, new FuncPredicate(() => moveController.IsJumping && !battleContorller.InBattleState));

        //Skills
        At(locomotion, skillState, new FuncPredicate(() => moveController.Grounded() && battleContorller.InBattleState));

        Any(locomotion, new FuncPredicate(() => !moveController.IsJumping && moveController.Grounded() && !battleContorller.InBattleState));
        Any(landingState, new FuncPredicate(() => !moveController.IsJumping && !moveController.Grounded()));
        stateMachine.SetState(locomotion);       
    }

    private void Update() {
        if (gameOnPause) return;
        stateMachine?.Update();
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAny(to, condition);

    public void OnGamePaused() {
        gameOnPause = true;
    }
    public void OnGameResume() {
        gameOnPause = false;
    }
}
