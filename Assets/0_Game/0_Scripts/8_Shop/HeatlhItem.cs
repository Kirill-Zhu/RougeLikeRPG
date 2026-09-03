using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/Shop/HealthItem")]
public class HeatlhItem : Item {
    public int AddMaxHealth = 1;

    public void Visit(HealthComponent healthComponent) {
        healthComponent.AddItem(this);
    }
}
