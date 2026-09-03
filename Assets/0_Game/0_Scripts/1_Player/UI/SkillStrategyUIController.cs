using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillStrategyUIController : MonoBehaviour, InGameUI {

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

    #region InGameUI
    [Header("Sho Hide Animaiton settings")]
    [SerializeField] Vector2 showPos;
    [SerializeField] Vector2 hidePos;
    [SerializeField] RectTransform transformToMove;
    Sequence sequence;
    [ContextMenu("Show UI")]
    public void ShowUI() {
        if (sequence != null)
            sequence.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(transformToMove.DOAnchorPos(showPos, InGameUI.duration)).SetEase(Ease.InFlash);

    }
    [ContextMenu("hide Ui")]
    public void HideUI() {
        if (sequence != null)
            sequence.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(transformToMove.DOAnchorPos(hidePos, InGameUI.duration)).SetEase(Ease.InFlash);
    }
    #endregion
}
