using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class LevelUpMenu : MonoBehaviour {
    Hero hero;
    [SerializeField] List<MeleSkillLevleUp> meleLevelUpList;
    [SerializeField] List<ShieldSkillLelvelUp> shieldLevelUpList;
    [SerializeField] List<ShootSkillLevelUp> shootLevelUpList;

    List<LevelUpStrategy> powerUpsToRise = new List<LevelUpStrategy>();

    [SerializeField] LvlUpCard leftCard;
    [SerializeField] LvlUpCard middleCard;
    [SerializeField] LvlUpCard rightCard;

    UnityEvent OnChooseCard;
    public void Initialize(Hero hero) {
        this.hero = hero;

        OnChooseCard = hero.OnChooseLelvelUpCard;
        OnChooseCard.AddListener(HideCards);    

        leftCard.Initialize(OnChooseCard);
        middleCard.Initialize(OnChooseCard);
        rightCard.Initialize(OnChooseCard);
    }

    void HideCards() {
        leftCard.gameObject.SetActive(false);
        middleCard.gameObject.SetActive(false);
        rightCard.gameObject.SetActive(false);
    }
    public void RiseLevelUp() {
    
        powerUpsToRise.Clear();

        var SkillStrategyArray = hero.HeroBattleController.SkillsStrategy;

        foreach (var skillStrategy in SkillStrategyArray) 
            GetRandomPowerUp(skillStrategy);
        

        leftCard.gameObject.SetActive(true);
        middleCard.gameObject.SetActive(true);
        rightCard.gameObject.SetActive(true);

        leftCard.Rise(SkillStrategyArray[0], powerUpsToRise[0]);
        middleCard.Rise(SkillStrategyArray[1], powerUpsToRise[1]);
        rightCard.Rise(SkillStrategyArray[2], powerUpsToRise[2]);

     
    }
    public void GetRandomPowerUp(object o) {
        MethodInfo visitMethod = GetType().GetMethod("GetRandomPowerUp", new Type[] { o.GetType() });
        if (visitMethod != null && visitMethod != GetType().GetMethod("GetRandomPowerUp", new Type[] { typeof(object) })) {
            visitMethod.Invoke(this, new object[] { o });
            Debug.Log($"GetRandomPowerUp : {o.GetType().Name} ");
        }
    }

    public void GetRandomPowerUp(MeleStrategy strategy) {
        int ranodm = UnityEngine.Random.Range(0, meleLevelUpList.Count);
        powerUpsToRise.Add(meleLevelUpList[ranodm]);
    }
    public void GetRandomPowerUp(ShieldStartegy strategy) {
        int ranodm = UnityEngine.Random.Range(0, shieldLevelUpList.Count);
        powerUpsToRise.Add(shieldLevelUpList[ranodm]);
    }
    public void GetRandomPowerUp(ShootStrategy strategy) {
        int ranodm = UnityEngine.Random.Range(0, shootLevelUpList.Count);
        powerUpsToRise.Add(shootLevelUpList[ranodm]);
    }
}
