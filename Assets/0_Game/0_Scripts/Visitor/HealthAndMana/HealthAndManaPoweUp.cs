using UnityEngine;

[CreateAssetMenu(menuName = "Visitor/HealthAndMana", fileName = "New HealthAndMana Power Up")]
public class HealthAndManaPoweUp : PowerUp {
    public int health;
    public int mana;

    public void Visit(HealthComponent HealthComponent) {
        HealthComponent.ChangeHealth(health);
    }
    public void Visit(ManaComponent manaComponent) {
        manaComponent.ChangeMana(mana);
    }
}