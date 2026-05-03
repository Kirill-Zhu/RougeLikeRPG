using UnityEngine;
using Zenject;
using System.Collections.Generic;
[RequireComponent(typeof(HealthComponentUI))]
public class InGameUIManager : MonoBehaviour {

    [Inject, SerializeField] Hero hero;
    [SerializeField] List<GameObject> startScreenObjects;
    [SerializeField] HealthComponentUI healthComponentUI;
    [SerializeField] HealthAndManaGlobesUI globes;
    [SerializeField] SkillStrategyUIController skillStrategyUIController;
    [SerializeField] LevelUpMenu levelUpMenu;
    [SerializeField] ExpBarUIContorller expBarUIContorller;
    [SerializeField] CoinControllerUI coinControllerUI;
    [SerializeField] AutoSkillUIController autoSkillController;
    [SerializeField] PowerUpMenu powerUpMenu;
    private void Awake() {


        //Get
        healthComponentUI = GetComponent<HealthComponentUI>();
        autoSkillController = GetComponent<AutoSkillUIController>();
        expBarUIContorller = GetComponent<ExpBarUIContorller>();
        coinControllerUI = GetComponent<CoinControllerUI>();
        powerUpMenu = GetComponent<PowerUpMenu>();

        //Events
        hero.OnHeroChange.AddListener(UpdateValues);
        hero.HealthComponent.OnTakeDamage += healthComponentUI.PopUpDamagePoints;
        hero.HealthComponent.OnGetCurrentHealth += globes.SetCurrentHealth;
        hero.ManaComponent.OnGetCurrentMana += globes.SetCurrentMana;
        hero.OnLevelUp.AddListener(RiseLevelUpMenu);

        //Initialize 
        autoSkillController.Initialize(hero.HeroAutoSkillContorller);
        expBarUIContorller.Initialize(hero);
        coinControllerUI.Initialaize(hero);
        powerUpMenu.Initialize(hero);

        //Destroy Start Screen Ojbects
        foreach(var obj in startScreenObjects) 
            Destroy(obj, 3);
    }

    private void Start() {
        //Initialize values
        globes.InitializeHealth(hero.HealthComponent.MaxHealth);
        globes.InitializeMana(hero.ManaComponent.MaxMana);
        skillStrategyUIController.Initialize(hero.HeroBattleController.SkillsStrategy);
        levelUpMenu.Initialize(hero);
        // levelUpMenu.Initialize(hero);

        //SkillStrategy 
        hero.HeroBattleController.SkillsStrategy[0].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallNorthSkill;
        hero.HeroBattleController.SkillsStrategy[1].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallWestSkill;
        hero.HeroBattleController.SkillsStrategy[2].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallEastSkill;

        //Additional Skills
    }
    private void OnDestroy() {
        hero.HealthComponent.OnTakeDamage -= healthComponentUI.PopUpDamagePoints;
        hero.HealthComponent.OnGetCurrentHealth -= globes.SetCurrentHealth;
        hero.ManaComponent.OnGetCurrentMana -= globes.SetCurrentMana;
        //SkillsStrategy
        hero.HeroBattleController.SkillsStrategy[0].OnCoolDownFillAmountValue -= skillStrategyUIController.OnCoolDownCallNorthSkill;
        hero.HeroBattleController.SkillsStrategy[1].OnCoolDownFillAmountValue -= skillStrategyUIController.OnCoolDownCallWestSkill;
        hero.HeroBattleController.SkillsStrategy[2].OnCoolDownFillAmountValue -= skillStrategyUIController.OnCoolDownCallEastSkill;

        //Additional Skills
    }

    void UpdateValues() {

        //Initialize values
        globes.InitializeHealth(hero.HealthComponent.MaxHealth);
        skillStrategyUIController.Initialize(hero.HeroBattleController.SkillsStrategy);

        //SkillStrategy 
        hero.HeroBattleController.SkillsStrategy[0].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallNorthSkill;
        hero.HeroBattleController.SkillsStrategy[1].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallWestSkill;
        hero.HeroBattleController.SkillsStrategy[2].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallEastSkill;

        //Additional Skills

        //Debug.Log("On Update hero invokes");
    }

    //LVlup
    public void RiseLevelUpMenu(int MaxEppValue) {
        levelUpMenu.RiseLevelUp();
    }
    //---
}
