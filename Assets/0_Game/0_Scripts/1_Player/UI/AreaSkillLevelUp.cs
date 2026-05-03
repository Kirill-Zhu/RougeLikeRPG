using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/LvlUp/AreaSkill")]
public class AreaSkillLevelUp : LevelUpStrategy {
    [SerializeField] int physicsDamageDirectIncrease = 0;
    [SerializeField] int fireDamageDirectIncrease = 0;
    [SerializeField] int coldDamageDirectIncrease = 0;

    [SerializeField] int physicsPecrent = 0;
    [SerializeField] int firePecrent = 0;
    [SerializeField] int coldPecrent = 0;

    [Header("Area Specifics")]
    [SerializeField] float radius = 1;
    [SerializeField] float recuceLiveDuration = 0;
    [SerializeField] float increaseLiveDuration = 0;
    [SerializeField] float reduceCoolDown = 0;
    [SerializeField] float increaseCoolDown = 0;
    public AreaStrategy UpgradeSkill(AreaStrategy area) {

        //Radius
        area.Radius += radius;

        //Live Duration
        area.LiveDuration += increaseLiveDuration;
        area.LiveDuration -= recuceLiveDuration;

        //CoolDown
        area.CoolDown += increaseCoolDown;
        area.CoolDown -= reduceCoolDown;

        //Damage
        damageTypesArray = GetDamageTypes(physicsDamageDirectIncrease + (int)(area.PhysicsDamage / 100 * physicsPecrent), fireDamageDirectIncrease + (int)(area.FireDamage / 100 * firePecrent), coldDamageDirectIncrease + (int)(area.ColdDamage / 100 * coldPecrent));
        area.AddOrModifyDamageType(damageTypesArray);
        return area;
    }
}