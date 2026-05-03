using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class HeroBattleController : MonoBehaviour, IVisitable {
    [SerializeField] HealthComponent healthComponent;
    public SkillsStrategy[] SkillsStrategy => skillStrategy;
    [SerializeField] SkillsStrategy[] skillStrategy = new SkillsStrategy[3];
    public UnityEvent<Sprite, string, string> OnPickUpPowerUp;
    const string interactionTagName = "Enemy";

    [SerializeField] InputReader inputs;
    public event Action<int, float> OnAnimationStart;
    public event UnityAction<int> OnManaChange;
    private event Action<float> OnSkillDurationChange;
    public bool InBattleState = false;
    public float SkillDurationTimer;
    public int FitstInputIndex;
    private float enqueTime = 0.2f;                                                                            //Wait this time to set Battle State after Input and then cancel if conditions are not approach
    private float cancelTimer = default;
    ManaComponent manaComponent;
    //Audio
    AudioManager audioManager;



    public void Initialize(ManaComponent manaComponent, SkillsStrategy[] skillsStrategy, UnityEvent<Sprite, string, string> @OnPickUpPowerUpEvent, AudioManager audioManager) {

        this.manaComponent = manaComponent;

        OnPickUpPowerUp = @OnPickUpPowerUpEvent;
        //Debug.Log($"Initialize : {GetType().Name}");

        //--Remove Previous Startfies
        //if (skillStrategy != null)
        //    foreach (var skill in skillsStrategy)
        //        skill.Dispose();

        //--------------Create new Sctiptable Objects

        for (int i = 0; i < skillsStrategy.Length; i++) {
            this.skillStrategy[i] = Instantiate(skillsStrategy[i]);
        }

        //--------------initialize it
        foreach (var skillStrategy in skillStrategy)                                              //initializeAll strategies
            skillStrategy.Initialize(this.transform, audioManager, interactionTagName);

        AddSkillDependecies();
        //Events

        //Audio
        audioManager = this.audioManager;


        StartCoroutine(CheckForInitialization());
    }
    IEnumerator CheckForInitialization() {
        foreach (var skill in skillStrategy) {
            if (!skill.Initialized()) {
                yield return null;
            }
        }
        //Initialize Events
        inputs.IsUsingSkill += usingSkill => {
            if (usingSkill == true) {
                InBattleState = true;
                cancelTimer = 0.3f;                                                                            //Reset eqnque TImer
            }
        };


        OnManaChange = null;
        OnManaChange += manaComponent.ChangeMana;
        inputs.UseSkill += SetSkillIndex;
        OnSkillDurationChange += duration => SkillDurationTimer = duration;

        Debug.Log($"{GetType().Name} is initialized");

        yield break;
    }

    private void Update() {

        if ((cancelTimer > 0)) cancelTimer -= Time.deltaTime;
        if (cancelTimer <= 0 && SkillDurationTimer <= 0) InBattleState = false;                                 // reset enque battle state if state machine conditions doesent match requerements

        OnUpdate();
    }
    public void SubscribeInputs() {
        inputs.UseSkill += UseSkill;
    }
    public void UnSubscribeInputs() {
        inputs.UseSkill -= UseSkill;
    }
    public void HandleUpdateStatus() {
        if (SkillDurationTimer > 0) SkillDurationTimer -= Time.deltaTime;                                     //Updates skill state
        if (SkillDurationTimer <= 0)
            InBattleState = false;
    }
    public void UseSkill(int index) {

        if (SkillDurationTimer > enqueTime || manaComponent.CurrentMana - skillStrategy[index].ManaCost < 0) return;
        SkillDurationTimer = 1;//Wait unitl ohter skill complete

        skillStrategy[index].TryUseSkill(OnSkillDurationChange, OnAnimationStart, OnManaChange);
    }
    void SetSkillIndex(int index) {
        Debug.Log("Set index");
        FitstInputIndex = index;
    }
    void AddSkillDependecies() {
        foreach (var strategy in skillStrategy) {
            strategy.HealthComponent = healthComponent;
        }
    }
    public void ChangeSkill(int skillIndex) {
        AddSkillDependecies();
        //Logic
    }
    private void OnUpdate() {
        //Start Skills Update
        foreach (var strategy in skillStrategy) {
            strategy.OnUpdate(Time.deltaTime);
        }

    }

    private void OnDestroy() {
        Dispose();
    }
    private void OnDisable() {
        Dispose();
    }
    public void Dispose() {
        UnSubscribeInputs();
        OnSkillDurationChange -= duration => SkillDurationTimer = duration;
        OnAnimationStart = null;
        OnManaChange = null;
        inputs.UseSkill -= SetSkillIndex;

        foreach (var strategy in skillStrategy) {
            strategy.Dispose();
        }
    }
    //Visitor
    public void PickUpPowerUp(Sprite label, string description, string name) {
        OnPickUpPowerUp.Invoke(label, description, name);
    }
    public void Accept(IVistor visitor) {
        visitor.Visit(this);

        foreach (var skill in SkillsStrategy) {
            visitor.Visit(skill);
        }
    }
}
