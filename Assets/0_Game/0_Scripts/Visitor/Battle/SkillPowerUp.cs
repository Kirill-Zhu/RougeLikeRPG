using UnityEngine;

[CreateAssetMenu(menuName = "Visitor/SkillLVlUp", fileName = "New Skill Lvl Up")]
public class SkillPowerUp : PowerUp {

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

    public void Visit(MeleStrategy meleStrategy) {
        meleStrategy.PhysicsDamage += MelePhysicsDamage;
        meleStrategy.FireDamage += MeleFireDamage;
        meleStrategy.ColdDamage += MeleColdDamage;

        meleStrategy.UpdateValues();
    }
    public void Visit(ShieldStartegy shieldStrategy) {

        shieldStrategy.PhysicsDamage += ShieldPhysicsDamage;
        shieldStrategy.FireDamage += ShieldFireDamage;
        shieldStrategy.ColdDamage += ShieldColdDamage;

        shieldStrategy.UpdateValues();
    }
    public void Visit(ShootStrategy shootStrategy) {
        shootStrategy.PhysicsDamage += ShootPhysicsDamage;
        shootStrategy.FireDamage += ShootFireDamage;
        shootStrategy.ColdDamage += ShootColdDamage;

        shootStrategy.UpdateValues();

    }
}