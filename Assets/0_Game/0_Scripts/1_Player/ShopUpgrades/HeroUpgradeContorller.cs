using UnityEngine;

public class HeroUpgradeContorller : MonoBehaviour {
    [SerializeField] ItemStrategy[] ItemStrategyArray;

    HealthComponent healthComponent;
    ManaComponent manaComponent;
    SimpleCahracterController characterController;
    HeroBattleController heroBattleController;
    public void Initialize(HealthComponent healthComponent, ManaComponent manaComponent, SimpleCahracterController contorller, HeroBattleController battle) {
        this.healthComponent = healthComponent;
        this.manaComponent = manaComponent;
        this.characterController = contorller;
        this.heroBattleController = battle;

        SetUpUpgrades();
    }

    void SetUpUpgrades() {

        characterController.ClearItems();
        healthComponent.ClearItems();
        foreach (var item in ItemStrategyArray) {
            healthComponent.Accept(item.CurrentItem());
            characterController.Accept(item.CurrentItem());
        }

        characterController.RefreshItemUpgrades();
        healthComponent.RefreshItemUpgrades();
    }
    void RefreshUpgrades() {

    }
}
