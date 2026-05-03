using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HeroAutoSkillController : MonoBehaviour, IVisitable {

    [SerializeField] List<AutoSkillStrategy> skillStrategyList;
    public UnityAction<List<AutoSkillStrategy>> OnChangelSkillList;
  
   
    private void Awake() {
        foreach(var strategy in skillStrategyList) 
            strategy.Initialize(transform);

        OnChangelSkillList?.Invoke(skillStrategyList);
    }
    private void OnEnable() {
        Dispose();
    }
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }
    public void AddSkill(AutoSkillStrategy skill) {

        var obj = Instantiate(skill);

        skillStrategyList.Add(obj);

        obj.Initialize(transform);

        OnChangelSkillList?.Invoke(skillStrategyList);
    }
    public void RemoveSKill() {

    }
    void Dispose() {
        OnChangelSkillList = null;  
        skillStrategyList.Clear();
    }
    public void OnUpdate() {

        foreach (var skillStrategy in skillStrategyList) {
            skillStrategy.OnUpdate(Time.deltaTime);
        }
    }
    
}
