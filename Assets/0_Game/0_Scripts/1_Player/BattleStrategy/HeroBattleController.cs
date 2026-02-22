using System;
using UnityEngine;
using System.Reflection;
public class HeroBattleController : MonoBehaviour, IVisitable {
    [SerializeField] HealthComponent healthComponent;
    public SkillsStrategy[] SkillsStrategy => skillStrategy;
    [SerializeField] SkillsStrategy[] skillStrategy = new SkillsStrategy [3];
    [SerializeField] InputReader inputs;
    public event Action<int,float> OnAnimationStart;
    private event Action<float> OnSkillDurationChange;
    public bool InBattleState = false;
    public float SkillDurationTimer;
    public int FitstInputIndex;
    private float enqueTime = 0.2f;                                                                            //Wait this time to set Battle State after Input and then cancel if conditions are not approach
    private float cancelTimer = default;

    public void Initialize(SkillsStrategy[] skillsStrategy) {

 
        //--------------Create new Sctiptable Objects
                           
        for (int i = 0; i < skillsStrategy.Length; i++) {
            this.skillStrategy[i] = Instantiate(skillsStrategy[i]);
        }

        //--------------initialize it
        foreach (var skillStrategy in skillStrategy)                                              //initializeAll strategies
            skillStrategy.Initialize(this.transform);

        AddSkillDependecies();
        //Events
        inputs.IsUsingSkill += usingSkill => {
            if (usingSkill == true) {
                InBattleState = true;
                cancelTimer = 0.3f;                                                                            //Reset eqnque TImer
            }
        };
        inputs.UseSkill += index => FitstInputIndex = index;
        OnSkillDurationChange += duration => SkillDurationTimer = duration;

    }
    private void Awake() {
        //foreach (var skillStrategy in skillStrategy)                                              //initializeAll strategies
        //    skillStrategy.Initialize(this.transform);

        //AddSkillDependecies();
        ////Events
        //inputs.IsUsingSkill += usingSkill => {
        //    if (usingSkill == true) {
        //        InBattleState = true;
        //        cancelTimer = 0.3f;                                                                            //Reset eqnque TImer
        //    }
        //};
        //inputs.UseSkill += index => FitstInputIndex = index;
        //OnSkillDurationChange += duration => SkillDurationTimer = duration;
    }

    private void Update() {
        if ((cancelTimer > 0)) cancelTimer -= Time.deltaTime;                                                     
        if (cancelTimer <=0 && SkillDurationTimer <= 0) InBattleState = false;                                 // reset rnque battle state if state machine conditions doesent match requerements

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
        if (SkillDurationTimer > enqueTime) return;
        SkillDurationTimer = 1;//Wait unitl ohter skill complete
        skillStrategy[index].TryUseSkill(OnSkillDurationChange, OnAnimationStart);        
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
        foreach (var strategy in skillStrategy) {
            strategy.OnUpdate(Time.deltaTime);
        }
    }

    private void OnDestroy() {
        UnSubscribeInputs();
        OnSkillDurationChange -= duration => SkillDurationTimer = duration;
    }

    //Visitor
   
    public void Accept(IVistor visitor) {
        foreach (var skill in SkillsStrategy) { 
            visitor.Visit(skill);
        }
    }
}

