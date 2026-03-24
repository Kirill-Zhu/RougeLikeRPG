using UnityEngine;
[CreateAssetMenu(menuName = "Visitor/SkillLVlUp", fileName = "New Skill Lvl Up")]
public class SkillPowerUp : PowerUp {
    [Header("Additional Damage Types")]
    [SerializeField] DamageTypesEnum DamageTypesEnum;
    DamageType[] meleDamageTypesArray = new DamageType[3];
    DamageType[] shieldDamageTypesArray = new DamageType[3];
    DamageType[] shootDamageTypesArray = new DamageType[3];

    [Header("Mele strategy settings")]
    public int MelePhysicsDamage;
    public int MeleFireDamage;
    public int MeleColdDamage;

    [Header("Shield Strategy settings")]
    public int ShieldPhysicsDamage;
    public int ShieldFireDamage;
    public int ShieldColdDamage;

    [Header("Shoot Strategy settings")]
    public int ShootPhysicsDamage;
    public int ShootFireDamage;
    public int ShootColdDamage;


    private void Awake() {
        meleDamageTypesArray = GetDamageTypes(MelePhysicsDamage, MeleFireDamage, MeleColdDamage);
        shieldDamageTypesArray = GetDamageTypes(ShieldPhysicsDamage, ShieldFireDamage, ShieldColdDamage);
        shootDamageTypesArray = GetDamageTypes(ShootPhysicsDamage, ShootFireDamage, ShootColdDamage);
        Debug.Log($" shoot damage is  {shootDamageTypesArray[0].Value}");
    }
    public void Visit(HeroBattleController battleController) {
        battleController.PickUpPowerUp(Label, Descritpion);
    }
    public void Visit(MeleStrategy meleStrategy) {
        meleStrategy.AddOrModifyDamageType(meleDamageTypesArray);
        meleStrategy.UpdateValues();
    }
    public void Visit(ShieldStartegy shieldStrategy) {
        shieldStrategy.AddOrModifyDamageType(shieldDamageTypesArray);
        shieldStrategy.UpdateValues();
    }
    public void Visit(ShootStrategy shootStrategy) {
        shootStrategy.AddOrModifyDamageType(shootDamageTypesArray);
        shootStrategy.UpdateValues();
    }

    protected virtual DamageType[] GetDamageTypes(int PhysicsDamage, int FireDamage, int ColdDamage) => DamageTypesEnum switch {
        DamageTypesEnum.Physics => new DamageType[] { new PhysicsDamageType(PhysicsDamage) },
        DamageTypesEnum.Fire => new DamageType[] { new FireDamageType(FireDamage) },
        DamageTypesEnum.Cold => new DamageType[] { new ColdDamageType(ColdDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(ColdDamage) },

        _ => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(ColdDamage) },
    };
}

public class AdditionalSkill : PowerUp {

    [SerializeField] SkillsStrategy additionalSkill;

    public void Visit(HeroBattleController heroBattleController) {


    }
}