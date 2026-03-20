using UnityEngine;
[CreateAssetMenu(menuName = "Visitor/AutoSKillPowerUp", fileName = "New AutoSkill")]
public class AutoSkillPowerUp : PowerUp
{
    [SerializeField] AutoSkillStrategy autoSkill;
    public void Visit(HeroAutoSkillController controller) {
        controller.AddSkill(autoSkill);
    }
}
