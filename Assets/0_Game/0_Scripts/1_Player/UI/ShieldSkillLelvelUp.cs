using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/LvlUp/ShieldSkill")]
public class ShieldSkillLelvelUp : LevelUpStrategy {
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

    public ShieldStartegy UpgradeSkill(ShieldStartegy shield) {

        damageTypesArray = GetDamageTypes(physicsDamageDirectIncrease + (int)(shield.PhysicsDamage / 100 * physicsPecrent), fireDamageDirectIncrease + (int)(shield.FireDamage / 100 * firePecrent), coldDamageDirectIncrease + (int)(shield.ColdDamage / 100 * coldPecrent));
        shield.SkillDuration -= recuceSkillDuration;
        shield.AddOrModifyDamageType(damageTypesArray);
        return shield;
    }
}
