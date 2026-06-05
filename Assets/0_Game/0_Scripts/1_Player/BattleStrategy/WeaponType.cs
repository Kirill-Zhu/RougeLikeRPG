using UnityEngine;

[System.Serializable]
public abstract class WeaponType : MonoBehaviour {
    [SerializeField] DamageTypesEnum DamageTypesEnum;
    public DamageType[] BaseDamage => baseDamage;
    protected DamageType[] baseDamage;
    protected DamageType[] bonusDamage = new DamageType[] { new PhysicsDamageType(0), new FireDamageType(0), new ColdDamageType(0) };
    protected DamageType[] totalDamage = new DamageType[] { new PhysicsDamageType(0), new FireDamageType(0), new ColdDamageType(0) };
    [SerializeField] int PhysicsDamage;
    [SerializeField] int FireDamage;
    [SerializeField] int ColdDamage;
    [SerializeField] protected float pushPower;
    [SerializeField] protected string interactionTagName;

    private void Awake() {
        if (baseDamage == null)
            baseDamage = GetStartDamageTypes();
    }
    protected abstract void OnTriggerEnter(Collider other);

    protected virtual DamageType[] GetStartDamageTypes() => DamageTypesEnum switch {
        DamageTypesEnum.Physics => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(0), new ColdDamageType(0) },
        DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(0), new FireDamageType(FireDamage), new ColdDamageType(0) },
        DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(0), new FireDamageType(0), new ColdDamageType(ColdDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(0) },
        DamageTypesEnum.Physics | DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(0), new ColdDamageType(ColdDamage) },

        _ => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(ColdDamage) },
    };

    public void DoDamage(HealthComponent healt) {
        for (int i = 0; i < baseDamage.Length; i++) {
            totalDamage = new DamageType[] { new PhysicsDamageType(0), new FireDamageType(0), new ColdDamageType(0) };
            totalDamage[i].AddDamage(baseDamage[i].Value);
            totalDamage[i].AddDamage(bonusDamage[i].Value);
            healt.TakeDamage(totalDamage[i]);
        }
    }
    public virtual void AddBonusDamage(DamageType bonus) {
        for (int i = 0; i < bonusDamage.Length; i++) {
            if (bonusDamage[i].GetType() == bonus.GetType()) {
                bonusDamage[i].AddDamage(bonus.Value);
            }
        }
    }

    public virtual void RemoveBonusDamage(DamageType bonus) {

        for (int i = 0; i < bonusDamage.Length; i++) {
            if (bonusDamage[i].GetType() == bonus.GetType()) {
                bonusDamage[i].RemoveBonus(bonus.Value);
            }
        }
    }
}
public enum WeaponTypeEnum {
    mele
    , projectile
    , area
}