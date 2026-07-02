using UnityEngine;
[CreateAssetMenu(menuName = "Visitor/SkillLVlUp", fileName = "New Skill Lvl Up")]
public class SkillPowerUp : PowerUp, IItem {
    [SerializeField] string itemName;
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

    //IItem
    public Sprite Icon { get => Label; set { } }
    public string Description { get => "Test"; set { } }

    public void Visit(HeroBattleController battleController) {
        battleController.PickUpPowerUp(Label, Descritpion, itemName);
    }
    public void Visit(MeleStrategy meleStrategy) {
        meleDamageTypesArray = GetDamageTypes(MelePhysicsDamage, MeleFireDamage, MeleColdDamage);
        meleStrategy.AddOrModifyDamageType(meleDamageTypesArray);
        meleStrategy.UpdateValues();
    }
    public void Visit(ShieldStartegy shieldStrategy) {
        shieldDamageTypesArray = GetDamageTypes(ShieldPhysicsDamage, ShieldFireDamage, ShieldColdDamage);
        shieldStrategy.AddOrModifyDamageType(shieldDamageTypesArray);
        shieldStrategy.UpdateValues();
    }
    public void Visit(ShootStrategy shootStrategy) {
        shootDamageTypesArray = GetDamageTypes(ShootPhysicsDamage, ShootFireDamage, ShootColdDamage);
        shootStrategy.AddOrModifyDamageType(shootDamageTypesArray);
        shootStrategy.UpdateValues();
    }

    protected virtual DamageType[] GetDamageTypes(int PhysicsDamage, int FireDamage, int ColdDamage) => DamageTypesEnum switch {
        DamageTypesEnum.Physics => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(0), new ColdDamageType(0) },
        DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(0), new FireDamageType(FireDamage), new ColdDamageType(0) },
        DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(0), new FireDamageType(0), new ColdDamageType(ColdDamage) },
        DamageTypesEnum.Physics | DamageTypesEnum.Fire => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(0) },
        DamageTypesEnum.Physics | DamageTypesEnum.Cold => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(0), new ColdDamageType(ColdDamage) },

        _ => new DamageType[] { new PhysicsDamageType(PhysicsDamage), new FireDamageType(FireDamage), new ColdDamageType(ColdDamage) },
    };
}

public class AdditionalSkill : PowerUp {

    [SerializeField] SkillsStrategy additionalSkill;

    public void Visit(HeroBattleController heroBattleController) {


    }
}