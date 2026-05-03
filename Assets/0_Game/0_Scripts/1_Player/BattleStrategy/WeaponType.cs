using UnityEngine;

[System.Serializable]
public abstract class WeaponType : MonoBehaviour {
    [SerializeField] DamageTypesEnum DamageTypesEnum;
    public DamageType[] DamageTypes => DamageTypes;
    protected DamageType[] damageTypes;
    protected DamageType[] bonusDamageTypes = new DamageType[] { new PhysicsDamageType(0), new FireDamageType(0), new ColdDamageType(0) };
    protected DamageType[] totalDamage = new DamageType[] { new PhysicsDamageType(0), new FireDamageType(0), new ColdDamageType(0) };
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
    public virtual void AddBonusDamage(DamageType bonus) {
        for (int i = 0; i < bonusDamageTypes.Length; i++) {
            if (bonusDamageTypes[i].GetType() == bonus.GetType()) {
                bonusDamageTypes[i].AddDamage(bonus.Value);
            }

        }
    }
    public virtual void RemoveBonusDamage(DamageType bonus) {

        for (int i = 0; i < bonusDamageTypes.Length; i++) {
            if (bonusDamageTypes[i].GetType() == bonus.GetType()) {
                bonusDamageTypes[i].RemoveBonus(bonus.Value);
            }

        }
    }
}
public enum WeaponTypeEnum {
    mele,
    projectile
}