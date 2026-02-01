using NUnit.Framework.Interfaces;
using UnityEngine;

public class HeroBattleController : MonoBehaviour {
    [SerializeField] SkillsStrategy[] skillStrategy;
    [SerializeField] InputReader inputs;
    public bool IsUsingSkill = false;
    public float SkillDuaration;
    public int FitstInputIndex;
    private void Awake() {
      
        inputs.IsUsingSkill += usingSkill => {
            if (usingSkill == true) {
                IsUsingSkill = true;
            }
        };
        inputs.UseSkill += index => FitstInputIndex = index;
    }

    private void Update() {
        OnUpdate(); 
    }
    public void SubscribeInputs() {
        inputs.UseSkill += UseSkill;
    }
    public void UnSubscribeInputs() {
        inputs.UseSkill -= UseSkill;
    }
    public void HandleUpdateStatus() {
        if (SkillDuaration > 0) SkillDuaration -= Time.deltaTime;  //Updates skill state
        if (SkillDuaration <= 0)
            IsUsingSkill = false;
    }
    public void UseSkill(int index) {

        skillStrategy[index].UseSkill(this.transform, out SkillDuaration);
    }
    private void OnUpdate() {
        foreach (var strategy in skillStrategy) {
            strategy.OnUpdate(Time.deltaTime);
        }
    }
    private void OnDestroy() {
        UnSubscribeInputs();
    }
}

