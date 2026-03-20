using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/LvlUp/ShootSkill")]
public class ShootSkillLevelUp : LevelUpStrategy {
    [SerializeField] bool needChangeShape;
    [SerializeField] ShootShape newShootShape;
    [SerializeField] int plusProjectilesCount = 0;
    [SerializeField] float speedUpProjectile = 0;
    [SerializeField] int physicsDamageDirectIncrease = 0;
    [SerializeField] int fireDamageDirectIncrease = 0;
    [SerializeField] int coldDamageDirectIncrease = 0;

    [SerializeField] int physicsPecrent = 0;
    [SerializeField] int firePecrent = 0;
    [SerializeField] int coldPecrent = 0;

    [SerializeField] float recuceSkillDuration = 0;
    public override string GetDescription() {
        string skills = "";
        skills += skillDescription;
        return skills;

    }

    public ShootStrategy UpgradeSkill(ShootStrategy shoot) {

        damageTypesArray = GetDamageTypes(physicsDamageDirectIncrease + (int)(shoot.PhysicsDamage / 100 * physicsPecrent), fireDamageDirectIncrease + (int)(shoot.FireDamage / 100 * firePecrent), coldDamageDirectIncrease + (int)(shoot.ColdDamage / 100 * coldPecrent));
        shoot.SkillDuration -= recuceSkillDuration;
        if (needChangeShape)
            shoot.shootShape = newShootShape;
        shoot.projectilesCountByShoot += plusProjectilesCount;

        shoot.speed += speedUpProjectile;
        shoot.AddOrModifyDamageType(damageTypesArray);
        return shoot;
    }
}