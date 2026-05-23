using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface InGameUI {
    public const int duration = 1;
    public void ShowUI();
    public void HideUI();
}
[RequireComponent(typeof(HealthComponentUI))]
public class InGameUIManager : MonoBehaviour {

    [Inject, SerializeField] Hero hero;
    [SerializeField] HealthComponentUI healthComponentUI;
    [SerializeField] HealthAndManaGlobesUI globes;
    [SerializeField] SkillStrategyUIController skillStrategyUIController;
    [SerializeField] LevelUpMenu levelUpMenu;
    [SerializeField] ExpBarUIContorller expBarUIContorller;
    [SerializeField] CoinControllerUI coinControllerUI;
    [SerializeField] AutoSkillUIController autoSkillController;
    [SerializeField] StartScreenAnimationController startScreenAnimationController;

    List<InGameUI> UIList = new List<InGameUI>();

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

        UIList.Add(globes);
        UIList.Add(skillStrategyUIController);
        UIList.Add(expBarUIContorller);
        UIList.Add(coinControllerUI);
        UIList.Add(autoSkillController);

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

        OnStartNewGame();
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

    async void OnStartNewGame(int startAnimDuration = 3) {
        HideAllUI();
        startScreenAnimationController.ShowStartAniamtion();
        await UniTask.WaitForSeconds(startAnimDuration);
        startScreenAnimationController.HideStartAniamtion();
        ShowAllUI();
    }

    [ContextMenu("Show All UI")]
    public void ShowAllUI() {
        foreach (var ui in UIList)
            ui.ShowUI();
    }
    [ContextMenu("Hide All UI")]
    public void HideAllUI() {
        foreach (var ui in UIList)
            ui.HideUI();
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
