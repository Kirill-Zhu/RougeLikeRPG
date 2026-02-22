using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/LvlUp/ShieldSkill")]
public class ShieldSkillLelvelUp : LevelUpStrategy {
    public int physicsDamage = 0;
    public int fireDamage = 0;
    public int coldDamage = 0;

    public override string GetDescription() {
        string skills = "";
        skills += skillDescription;
        return skills;

    }
}
