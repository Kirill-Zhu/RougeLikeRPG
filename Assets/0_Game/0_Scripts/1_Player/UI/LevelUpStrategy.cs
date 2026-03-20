using UnityEngine;

public abstract class LevelUpStrategy : ScriptableObject {
    [SerializeField] DamageTypesEnum damageTypes;
    protected DamageType[] damageTypesArray;
    [TextArea(2, 10)]
    [SerializeField] protected string skillDescription;

 
    public virtual string GetDescription() {
        return skillDescription;
    }
    protected virtual DamageType[] GetDamageTypes(int PhysicsDamage, int FireDamage, int ColdDamage) => damageTypes switch {
        DamageTypesEnum.Physics => new DamageType[] { new PhysicsDamageType(PhysicsDamage) },
        DamageTypesEnum.Fire => new DamageType[] { new FireDamageType(FireDamage) },
        DamageTypesEnum.Cold => new DamageType[] { new ColdDamageType(ColdDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(ColdDamage) },

        _ => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(ColdDamage) },
    };
}
