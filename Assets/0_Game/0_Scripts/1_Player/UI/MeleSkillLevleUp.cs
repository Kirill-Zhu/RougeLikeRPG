using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/LvlUp/MeleSkill")]
public class MeleSkillLevleUp : LevelUpStrategy {
    [SerializeField] int physicsDamageDirectIncrease = 0;
    [SerializeField] int fireDamageDirectIncrease = 0;
    [SerializeField] int coldDamageDirectIncrease = 0;

    [SerializeField] int physicsPecrent = 0;
    [SerializeField] int firePecrent = 0;
    [SerializeField] int coldPecrent = 0;

    [SerializeField] float attackRange = 0;
    [SerializeField] float recuceSkillDuration = 0;

    private void Awake() {

    }
    public override string GetDescription() {
        string skills = "";
        skills += skillDescription;
        return skills;

    }

    public MeleStrategy UpgradeSkill(MeleStrategy mele) {

        damageTypesArray = GetDamageTypes(physicsDamageDirectIncrease + (int)(mele.PhysicsDamage / 100 * physicsPecrent), fireDamageDirectIncrease + (int)(mele.FireDamage / 100 * firePecrent), coldDamageDirectIncrease + (int)(mele.ColdDamage / 100 * coldPecrent));
        mele.AttackRange += attackRange;
        mele.SkillDuration -= recuceSkillDuration;

        mele.AddOrModifyDamageType(damageTypesArray);
        return mele;
    }
}
