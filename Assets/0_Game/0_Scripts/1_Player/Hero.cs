using MyStateMachine;
using UnityEngine;
using UnityEngine.Events;
public class Hero : MonoBehaviour {
    //Test
    public bool Initialaized = false;
    //Initialize
    public GameObject Model => model;
    GameObject model;

    public UnityEvent OnHeroChange;///Invokes Every time when need change UI
    public UnityEvent<int, int> OnGetExp;
    public UnityEvent<Sprite, string, string> OnPickUppowerUp;
    public UnityEvent<int> OnLevelUp;
    public UnityEvent OnChooseLelvelUpCard;
    //Event Bus
    public UnityEvent OnDie;
    HeroStrategyData heroData;

    //Movement
    SimpleCahracterController moveController;
    #region ANDROID
    public void SetJoystick(DynamicJoystick joystick) {
        moveController.SetUpAndoridJoystick(joystick);
    }
    #endregion

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
    ManaComponent manaComponent;


    //Expiriance
    public ExpComponent ExpComponent => expComponent;
    ExpComponent expComponent;

    //Upgrade
    HeroUpgradeContorller upgradeContorller;
    //Power Up 
    public CoinsComponent CoinsComponent => coinsComponent;
    CoinsComponent coinsComponent;
    //-> Shop
    Item[] shopItems;
    //State Machine
    [SerializeField] Animator animator;
    StateMachine stateMachine;
    PausedState pausedState;
    Locomotion locomotion;
    JumpState jumpState;
    LandingState landingState;
    SkillState skillState;

    //Audio
    [SerializeField] AudioManager audioManager;

    #region EVENTS
    //-> EventBus
    EventBinding<OnUpgradeItemInShop> onUpgradeItemInShop;
    //<-EventBus
    public bool Paused => paused;
    bool paused = false;
    EventManager eventManager;
    #endregion
    public bool initialization = true;


    private void Awake() {
        moveController = GetComponent<SimpleCahracterController>();
        battleContorller = GetComponent<HeroBattleController>();
        heroAutoSkillController = GetComponent<HeroAutoSkillController>();
        healthComponent = GetComponent<HealthComponent>();
        manaComponent = GetComponent<ManaComponent>();
        expComponent = GetComponent<ExpComponent>();
        upgradeContorller = GetComponent<HeroUpgradeContorller>();
        coinsComponent = GetComponent<CoinsComponent>();
    }
    private void OnEnable() {
        //Event Bus

        onUpgradeItemInShop = new EventBinding<OnUpgradeItemInShop>(GetItemsFromShop);
        EventBus<OnUpgradeItemInShop>.Register(onUpgradeItemInShop);
    }
    private void OnDisable() {
        Initialaized = false;

        //EventBus
        EventBus<OnUpgradeItemInShop>.Deregister(onUpgradeItemInShop);
    }
    //EventBus
    void GetItemsFromShop(OnUpgradeItemInShop items) {
        Debug.Log($"Hero get call from {items.GetType().Name}");
    }
    public void Initialize(HeroStrategyData data) {

        //Data
        heroData = data;

        if (model != null) Destroy(model.gameObject);

        model = Instantiate(heroData.ModelPrefab, transform);
        model.transform.localPosition = Vector3.zero;

        animator = model.GetComponent<Animator>();
        //Health
        healthComponent.Initialize(heroData.HealtComponentData);
        healthComponent.OnDie += Die;
        //Mana
        manaComponent.Initialize(heroData.ManaConponentData);
        //Battle

        battleContorller.Initialize(manaComponent, heroData.SkillStrategyData, OnPickUppowerUp, audioManager);

        //Exp
        expComponent.Initialize(OnLevelUp, OnGetExp);

        //Upgrade   
        upgradeContorller.Initialize(healthComponent, manaComponent, moveController, battleContorller);

        //Pick Up
        coinsComponent.Initialaize(this);

        OnHeroChange?.Invoke();

        //StateMachine
        stateMachine = new StateMachine();
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
        Initialaized = true;

    }


    private void Update() {
        if (!Initialaized) return;
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

    //Events 
    public void SetEventManager(EventManager eventManager) {
        this.eventManager = eventManager;
        eventManager.OnLoadMainMenu.AddListener(() => gameObject.SetActive(false));

    }

}
