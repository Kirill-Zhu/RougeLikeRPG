using MyStateMachine;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
public class Hero : MonoBehaviour {

    //Initialize
    public UnityEvent OnHeroChange;///Invokes Every time when need change UI
    public UnityEvent<int, int> OnGetExp;
    public UnityEvent<Sprite, string, string> OnPickUppowerUp;
    public UnityEvent<int> OnLevelUp;
    public UnityEvent OnChooseLelvelUpCard;
    public UnityEvent OnDie;
    HeroStrategyData heroData;

    SimpleCahracterController moveController;

    //Battle
    public HeroBattleController HeroBattleController => battleContorller;
    HeroBattleController battleContorller;

    public HeroAutoSkillController HeroAutoSkillContorller => heroAutoSkillController;
    HeroAutoSkillController heroAutoSkillController;
    //Health
    public HealthComponent HealthComponent => healthComponent;
    [SerializeField] HealtComponentData healthData;
    HealthComponent healthComponent;

    //Mana
    public ManaComponent ManaComponent => manaComponent;
    [SerializeField] ManaComponent manaComponent;
    //Expiriance
    public ExpComponent ExpComponent => expComponent;
    ExpComponent expComponent;
    //Power Up 

    //State Machine
    StateMachine stateMachine = new StateMachine();
    [SerializeField] Animator animator;
    PausedState pausedState;
    Locomotion locomotion;
    JumpState jumpState;
    LandingState landingState;
    SkillState skillState;

    //Audio
    [SerializeField] AudioManager audioManager;
    public bool Paused => paused;
    bool paused = false;

    public void Initialize(HeroStrategyData data) {
        heroData = data;

        //Health
        healthComponent.Initialize(heroData.HealtComponentData);
        healthComponent.OnDie += Die;

        //Battle
        battleContorller.Initialize(manaComponent, heroData.SkillStrategyData, OnPickUppowerUp, audioManager);

        //Exp
        expComponent.Initialize(OnLevelUp, OnGetExp);

        OnHeroChange?.Invoke();
    }

    private void Awake() {
        moveController = GetComponent<SimpleCahracterController>();
        battleContorller = GetComponent<HeroBattleController>();
        heroAutoSkillController = GetComponent<HeroAutoSkillController>();
        healthComponent = GetComponent<HealthComponent>();
        manaComponent = GetComponent<ManaComponent>();
        expComponent = GetComponent<ExpComponent>();

        //StateMachine
        pausedState = new PausedState(moveController, animator, battleContorller, heroAutoSkillController);
        locomotion = new Locomotion(moveController, animator, battleContorller, heroAutoSkillController);
        jumpState = new JumpState(moveController, animator, battleContorller, heroAutoSkillController);
        landingState = new LandingState(moveController, animator, battleContorller, heroAutoSkillController);
        skillState = new SkillState(moveController, animator, battleContorller, heroAutoSkillController);



        //Movement
        At(locomotion, jumpState, new FuncPredicate(() => moveController.IsJumping));
        At(landingState, locomotion, new FuncPredicate(() => !moveController.IsJumping && moveController.Grounded()));
        At(skillState, jumpState, new FuncPredicate(() => moveController.IsJumping && !battleContorller.InBattleState));
        At(pausedState, locomotion, new FuncPredicate(() => true));

        //Skills
        At(locomotion, skillState, new FuncPredicate(() => moveController.Grounded() && battleContorller.InBattleState));

        //Any
        Any(pausedState, new FuncPredicate(() => paused));
        Any(locomotion, new FuncPredicate(() => !moveController.IsJumping && moveController.Grounded() && !battleContorller.InBattleState));
        Any(landingState, new FuncPredicate(() => !moveController.IsJumping && !moveController.Grounded()));
        stateMachine.SetState(locomotion);
    }

    private void Update() {

        stateMachine?.Update();
    }
    void Die() {
        OnDie?.Invoke();
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAny(to, condition);

    public void OnGamePaused() {
        paused = true;

    }
    public void OnGameResume() {
        paused = false;
        stateMachine.SetState(locomotion);
    }
}
