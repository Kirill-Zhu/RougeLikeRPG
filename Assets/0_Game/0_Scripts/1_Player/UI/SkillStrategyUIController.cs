using UnityEngine;
using UnityEngine.UI;

public class SkillStrategyUIController : MonoBehaviour {

    [SerializeField] Image[] skillsIconsArray;
    [SerializeField] Image northFillImage;
    [SerializeField] Image westFillImage;
    [SerializeField] Image eastFillImage;

    public void Initialize(SkillsStrategy[] skillsStrategies) {
        for (int i = 0; i < skillsStrategies.Length; i++) {
            skillsIconsArray[i].sprite = skillsStrategies[i].Icon;
        }
    }
    public void OnCoolDownCallWestSkill(float value) {
        westFillImage.fillAmount = value;
    }
    public void OnCoolDownCallNorthSkill(float value) {
        northFillImage.fillAmount = value;
    }

    public void OnCoolDownCallEastSkill(float value) {
        eastFillImage.fillAmount = value;
    }
}
