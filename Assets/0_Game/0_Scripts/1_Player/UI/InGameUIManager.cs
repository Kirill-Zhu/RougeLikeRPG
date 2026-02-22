using UnityEngine;
using Zenject;
[RequireComponent(typeof(HealthComponentUI))]
public class InGameUIManager : MonoBehaviour {

    [Inject, SerializeField] Hero hero;
    [SerializeField] HealthComponentUI healthComponentUI;
    [SerializeField] HealthAndManaGlobesUI globes;
    [SerializeField] SkillStrategyUIController skillStrategyUIController;
    [SerializeField] LevelUpMenu levelUpMenu;
    private void Awake() {


        //Initialize
        healthComponentUI = GetComponent<HealthComponentUI>();


        //Events
        hero.OnHeroChange.AddListener(UpdateValues);
        hero.HealthComponent.OnTakeDamage += healthComponentUI.PopUpDamagePoints;
        hero.HealthComponent.OnGetCurrentHealth += globes.ChangeHealth;
        hero.OnLevelUp.AddListener(RiseLevelUpMenu);
    }

    private void Start() {
        //Initialize values
        globes.InitializeHealth(hero.HealthComponent.MaxHealth);
        skillStrategyUIController.Initialize(hero.HeroBattleController.SkillsStrategy);
        levelUpMenu.Initialize(hero);
        // levelUpMenu.Initialize(hero);

        //SkillStrategy 
        hero.HeroBattleController.SkillsStrategy[0].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallNorthSkill;
        hero.HeroBattleController.SkillsStrategy[1].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallWestSkill;
        hero.HeroBattleController.SkillsStrategy[2].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallEastSkill;
    }
    private void OnDestroy() {
        hero.HealthComponent.OnTakeDamage -= healthComponentUI.PopUpDamagePoints;
        hero.HealthComponent.OnGetCurrentHealth -= globes.ChangeHealth;

        //SkillsStrategy
        hero.HeroBattleController.SkillsStrategy[0].OnCoolDownFillAmountValue -= skillStrategyUIController.OnCoolDownCallNorthSkill;
        hero.HeroBattleController.SkillsStrategy[1].OnCoolDownFillAmountValue -= skillStrategyUIController.OnCoolDownCallWestSkill;
        hero.HeroBattleController.SkillsStrategy[2].OnCoolDownFillAmountValue -= skillStrategyUIController.OnCoolDownCallEastSkill;
    }

    void UpdateValues() {

        //Initialize values
        globes.InitializeHealth(hero.HealthComponent.MaxHealth);
        skillStrategyUIController.Initialize(hero.HeroBattleController.SkillsStrategy);

        //SkillStrategy 
        hero.HeroBattleController.SkillsStrategy[0].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallNorthSkill;
        hero.HeroBattleController.SkillsStrategy[1].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallWestSkill;
        hero.HeroBattleController.SkillsStrategy[2].OnCoolDownFillAmountValue += skillStrategyUIController.OnCoolDownCallEastSkill;

        //Debug.Log("On Update hero invokes");
    }

    //Level UP
    [ContextMenu("Rise LVl up")]
    public void RiseLevelUpMenu() {
        levelUpMenu.RiseLevelUp();
    }
    //---
}
