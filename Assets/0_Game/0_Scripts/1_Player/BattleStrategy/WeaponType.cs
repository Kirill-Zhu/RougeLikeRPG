using UnityEngine;

[System.Serializable]
public abstract class WeaponType : MonoBehaviour {
    [SerializeField] DamageTypesEnum DamageTypesEnum;
    public DamageType[] DamageTypes => DamageTypes;
    protected DamageType[] damageTypes;
    [SerializeField] int PhysicsDamage;
    [SerializeField] int FireDamage;
    [SerializeField] int ColdDamage;
    [SerializeField] protected float pushPower;
    [SerializeField] protected string interactionTagName;

    private void Awake() {
        if (damageTypes == null)
            damageTypes = GetStartDamageTypes();
    }
    protected abstract void OnTriggerEnter(Collider other);

    protected virtual DamageType[] GetStartDamageTypes() => DamageTypesEnum switch {
        DamageTypesEnum.Physics => new DamageType[] { new PhysicsDamageType(PhysicsDamage) },
        DamageTypesEnum.Fire => new DamageType[] { new FireDamageType(FireDamage) },
        DamageTypesEnum.Cold => new DamageType[] { new ColdDamageType(ColdDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(ColdDamage) },

        _ => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(ColdDamage) },
    };
}
public enum WeaponTypeEnum {
    mele,
    projectile
}