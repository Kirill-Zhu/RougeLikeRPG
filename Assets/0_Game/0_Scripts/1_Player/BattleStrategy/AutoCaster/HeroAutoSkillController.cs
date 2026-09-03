using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class HeroAutoSkillController : MonoBehaviour, IVisitable {

    [SerializeField] Dictionary<string, AutoSkillStrategy> skillStrategyDictionary = new Dictionary<string, AutoSkillStrategy>();
    public UnityAction<List<AutoSkillStrategy>> OnChangelSkillList;

    private void Awake() {
        if (skillStrategyDictionary.Count <= 0)
            return;
        foreach (var strategy in skillStrategyDictionary)
            strategy.Value.Initialize(transform);

        OnChangelSkillList?.Invoke(skillStrategyDictionary.Values.ToList());
    }
    private void OnEnable() {
        Dispose();
    }
    public void Accept(IVistor visitor) {
        visitor.Visit(this);
    }
    public void AddOrUpgradeSkill(AutoSkillStrategy skill) {



        if (skillStrategyDictionary.TryGetValue(skill.name, out var obj)) { //upgrage skill 

            Debug.Log($"Upgrade skill : {skill.name} ");
            obj.Dispose();
            obj = UpgradeSkill(obj);
            obj.Initialize(transform);
            skillStrategyDictionary[skill.name] = obj;
            OnChangelSkillList?.Invoke(skillStrategyDictionary.Values.ToList());
            return;
        }
        Debug.Log($"Add skill : {skill.name} ");
        //Add new skill
        var newObj = Instantiate(skill);
        newObj.Initialize(transform);
        skillStrategyDictionary[skill.name] = newObj;
        OnChangelSkillList?.Invoke(skillStrategyDictionary.Values.ToList());
    }
    AutoSkillStrategy UpgradeSkill(AutoSkillStrategy strategy) {
        var type = strategy.GetType();
        if (type == typeof(MeleAutocaster)) {
            strategy = strategy.UpgrageSkill<MeleAutocaster>(strategy as MeleAutocaster);
            return strategy;
        }
        if (type == typeof(ShootAutocaster)) {
            strategy = strategy.UpgrageSkill<ShootAutocaster>(strategy as ShootAutocaster);
            return strategy;
        }
        return strategy;
    }
    public void RemoveSKill() {

    }
    public void Dispose() {
        OnChangelSkillList = null;
        skillStrategyDictionary.Clear();
    }
    public void OnUpdate() {
        if (skillStrategyDictionary.Count <= 0)
            return;
        foreach (var skillStrategy in skillStrategyDictionary) {
            skillStrategy.Value.OnUpdate(Time.deltaTime);
        }
    }

}
