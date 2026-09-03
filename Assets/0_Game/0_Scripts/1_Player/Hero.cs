using Cysharp.Threading.Tasks.Triggers;
using MyStateMachine;
using R3;
using System;
using UnityEngine;
using UnityEngine.Events;
public class Hero : MonoBehaviour {
    //Test
    public bool Initialaized = false;
    //Initialize
    public GameObject Model => model;
    GameObject model;

    [HideInInspector] public UnityEvent OnHeroChange;///Invokes Every time when need change UI
    [HideInInspector] public UnityEvent<int, int> OnGetExp;
    [HideInInspector] public UnityEvent<Sprite, string, string> OnPickUpItemPowerUp;
    [HideInInspector] public UnityEvent<int> OnLevelUp;
    [HideInInspector] public UnityEvent OnChooseLelvelUpCard;

    HeroStrategyData heroData;


    //R3
    public ReactiveProperty<bool> IsActive = new ReactiveProperty<bool>(false);
    //
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

    //UI
    [Header("UI Properties")]
    [SerializeField] PointerController pointerController;
    Vector2 missionTarget = new Vector3(30, 0, 40);

    //Renderer
    HeroRendererController heroRendererController;
    //
    //State Machine
    [SerializeField] Animator animator;
    StateMachine stateMachine;
    SafeZoneState safeZoneState;
    PausedState pausedState;
    Locomotion locomotion;
    JumpState jumpState;
    StrafeState strafeState;
    LandingState landingState;
    SkillState skillState;

    //Audio
    [SerializeField] HeroAudioManager audioManager;

    #region EVENTS
    //-> EventBus
    EventBinding<OnUpgradeItemInShop> onUpgradeItemInShopBinding;
    EventBinding<OnSafeZone> OnSafeZoneBinding;
    EventBinding<OnPlayerRessurect> OnPlayerRessurectBinding;
    //<-EventBus
    public bool Paused => paused;
    bool paused = false;
    public bool CutScene => safeZone;
    bool safeZone;
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
        audioManager = GetComponent<HeroAudioManager>();
        heroRendererController = GetComponent<HeroRendererController>();    
       
        IsActive.Subscribe(newValue => {
            battleContorller.IsActive = newValue;
        });
        IsActive.Value = false;
    }
    private void OnEnable() {
        //Event Bus

        onUpgradeItemInShopBinding = new EventBinding<OnUpgradeItemInShop>(GetItemsFromShop);
        EventBus<OnUpgradeItemInShop>.Register(onUpgradeItemInShopBinding);

        OnSafeZoneBinding = new EventBinding<OnSafeZone>(OnSafeZoneState);
        EventBus<OnSafeZone>.Register(OnSafeZoneBinding);

        OnPlayerRessurectBinding = new EventBinding<OnPlayerRessurect>(RessurectHero);
        EventBus<OnPlayerRessurect>.Register(OnPlayerRessurectBinding);
    }

    

    private void OnDisable() {
        Initialaized = false;

        //EventBus
        EventBus<OnUpgradeItemInShop>.Deregister(onUpgradeItemInShopBinding);
        EventBus<OnSafeZone>.Deregister(OnSafeZoneBinding);
        EventBus<OnPlayerRessurect>.Deregister(OnPlayerRessurectBinding);
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

        battleContorller.Initialize(manaComponent, heroData.SkillStrategyData, OnPickUpItemPowerUp, audioManager);

        //Exp
        expComponent.Initialize(OnLevelUp, OnGetExp);

        //Upgrade   
        upgradeContorller.Initialize(healthComponent, manaComponent, moveController, battleContorller);

        //Pick Up
        coinsComponent.Initialaize(this);
        //Renderer
        heroRendererController.Initialize(model.GetComponent<HeroModelHandler>());

        OnHeroChange?.Invoke();

        //StateMachine
        stateMachine = new StateMachine();
        safeZoneState = new SafeZoneState(moveController, animator, battleContorller, heroAutoSkillController, pointerController);
        pausedState = new PausedState(moveController, animator, battleContorller, heroAutoSkillController);
        locomotion = new Locomotion(moveController, animator, battleContorller, heroAutoSkillController);
       // jumpState = new JumpState(moveController, animator, battleContorller, heroAutoSkillController);
       // landingState = new LandingState(moveController, animator, battleContorller, heroAutoSkillController);
        skillState = new SkillState(moveController, animator, battleContorller, heroAutoSkillController);
        strafeState = new StrafeState(moveController, animator, battleContorller, heroAutoSkillController, heroRendererController) ;


        //Movement
       // At(locomotion, jumpState, new FuncPredicate(() => moveController.IsJumping));
      //  At(landingState, locomotion, new FuncPredicate(() => !moveController.IsJumping && moveController.Grounded()));
        At(locomotion, strafeState, new FuncPredicate(() => moveController.IsStrafing));
        At(skillState, strafeState, new FuncPredicate(() => moveController.IsStrafing && !battleContorller.InBattleState));
       // At(skillState, jumpState, new FuncPredicate(() => moveController.IsJumping && !battleContorller.InBattleState));
        At(pausedState, locomotion, new FuncPredicate(() => true));
        At(pausedState, skillState, new FuncPredicate(() => battleContorller.InBattleState));
        //Skills
        At(locomotion, skillState, new FuncPredicate(() => moveController.Grounded() && battleContorller.InBattleState));

        //Any
        Any(pausedState, new FuncPredicate(() => paused));
        Any(safeZoneState, new FuncPredicate(() => safeZone && !paused));
        Any(locomotion, new FuncPredicate(() => !moveController.IsStrafing && !battleContorller.InBattleState));
       // Any(landingState, new FuncPredicate(() => !moveController.IsStrafing && !moveController.Grounded()));
        stateMachine.SetState(locomotion);
        Initialaized = true;
        IsActive.Value = true;

    }


    private void Update() {
        if (!Initialaized || !IsActive.Value) return;
        stateMachine?.Update();
    }
    void Die() {
        EventBus<OnPlayerDied>.Raise(new OnPlayerDied { hero = this });
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
    public void ExitLevel() {
        IsActive.Value = false;
        paused = true;
        battleContorller.Dispose();
        heroAutoSkillController.Dispose();
    }
    [ContextMenu("Safe zone state")]
    public void OnSafeZoneState() {
        IsActive.Value = false;
        safeZone = true;
        stateMachine.SetState(safeZoneState);
    }
    [ContextMenu("Safe Zone Disable")]
    public void OnDangerZoneState() {
        IsActive.Value = true;  
        safeZone = false;
    }
    private void RessurectHero(OnPlayerRessurect ressurect) {
        stateMachine.SetState(locomotion);
    }
    //Events 
    public void SetEventManager(EventManager eventManager) {
        this.eventManager = eventManager;
        eventManager.OnLoadMainMenu.AddListener(() => gameObject.SetActive(false));

    }
    //UI UI Manager set up it itself 

}
