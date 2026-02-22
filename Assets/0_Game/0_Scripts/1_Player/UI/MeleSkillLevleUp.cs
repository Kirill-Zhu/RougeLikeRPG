using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/LvlUp/MeleSkill")]
public class MeleSkillLevleUp : LevelUpStrategy {


    [SerializeField] int physicsDamageDirectIncrease = 0;
    [SerializeField] int fireDamageDirectIncrease = 0;
    [SerializeField] int coldDamageDirectIncrease = 0;

    [SerializeField] int physicsPecrent = 1;
    [SerializeField] int firePecrent = 1;
    [SerializeField] int coldPecrent = 1;

    [SerializeField] float attackRange = 0;
    [SerializeField] float recuceSkillDuration = 0;

    public override string GetDescription() {
        string skills = "";
        skills += skillDescription;
        return skills;

    }

    public MeleStrategy UpgradeSkill(MeleStrategy mele) {
        mele.PhysicsDamage += physicsDamageDirectIncrease;
        mele.FireDamage += fireDamageDirectIncrease;
        mele.ColdDamage += coldDamageDirectIncrease;
        
        mele.PhysicsDamage *= physicsPecrent;
        mele.FireDamage *= firePecrent;
        mele.ColdDamage *= coldPecrent;

        mele.AttackRange += attackRange;
        mele.SkillDuration -= recuceSkillDuration;

        return mele;
    }
}
