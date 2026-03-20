using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/HealthCopmonentData", fileName = "New Heatlh Component Data")]
public class HealtComponentData : ScriptableObject
{
    public PopUpDamageValuesHandler popUpDamageValuesHandler;
    public int MaxHealth;
    public int PhysicsDefence;
    public int FireDefence;
    public int ColdDefence;
}
