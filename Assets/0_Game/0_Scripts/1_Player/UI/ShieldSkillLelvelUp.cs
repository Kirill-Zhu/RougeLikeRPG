using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/LvlUp/ShieldSkill")]
public class ShieldSkillLelvelUp : LevelUpStrategy {
    [SerializeField] int physicsDamageDirectIncrease = 0;
    [SerializeField] int fireDamageDirectIncrease = 0;
    [SerializeField] int coldDamageDirectIncrease = 0;

    [SerializeField] int physicsPecrent = 0;
    [SerializeField] int firePecrent = 0;
    [SerializeField] int coldPecrent = 0;
    [SerializeField] float recuceLiveDuration = 0;
    [SerializeField] float increaseLiveDuration = 0;
    [SerializeField] float reduceCoolDown = 0;
    [SerializeField] float increaseCoolDown = 0;
    public override string GetDescription() {
        string skills = "";
        skills += skillDescription;
        return skills;

    }

    public ShieldStartegy UpgradeSkill(ShieldStartegy shield) {

        //CoolDown
        shield.CoolDown -= increaseLiveDuration;
        if (shield.CoolDown < 0.1) shield.CoolDown = 0.1f; //Set minimum CoolDown 
        shield.CoolDown += increaseCoolDown;

        //LiveDuration
        shield.LiveDuration -= recuceLiveDuration;
        shield.LiveDuration += increaseLiveDuration;

        //Damage 
        damageTypesArray = GetDamageTypes(physicsDamageDirectIncrease + (int)(shield.PhysicsDamage / 100 * physicsPecrent), fireDamageDirectIncrease + (int)(shield.FireDamage / 100 * firePecrent), coldDamageDirectIncrease + (int)(shield.ColdDamage / 100 * coldPecrent));
        shield.AddOrModifyDamageType(damageTypesArray);
        return shield;
    }
}
