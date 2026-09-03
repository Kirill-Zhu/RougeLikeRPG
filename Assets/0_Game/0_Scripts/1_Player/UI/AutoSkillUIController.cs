using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class AutoSkillUIController : MonoBehaviour, InGameUI {

    [SerializeField] Transform parentUi;
    [SerializeField] Sprite fillSprite;
    [SerializeField] List<GameObject> skillObjectsList = new List<GameObject>();
    [SerializeField] List<Image> fillImageList = new List<Image>();
    HeroAutoSkillController heroAutoSkillContorller;


  


    public void Initialize(HeroAutoSkillController heroAutoSkillContorller) {
        this.heroAutoSkillContorller = heroAutoSkillContorller;
        heroAutoSkillContorller.OnChangelSkillList += UpdateValues;
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
    public void UpdateValues(List<AutoSkillStrategy> skillList) {

        //Clear
        for (int i = 0; i < skillList.Count; i++)
            skillList[i].OnCoolDownFillAmountValue = null;

        foreach (var obj in skillObjectsList)
            Destroy(obj.gameObject);
        skillObjectsList.Clear();
        fillImageList.Clear();

        //Add
        foreach (var strategy in skillList) {
            AddSkill(strategy);
        }

       
    }

    void AddSkill(AutoSkillStrategy strategy) {

       
        //Skill objects
        var skillObj = new GameObject();
        skillObj.transform.parent = parentUi;
        var image = skillObj.AddComponent<Image>();
        image.sprite = strategy.icon;
        skillObjectsList.Add(skillObj);
        //FillAmount
        var fillObj = new GameObject();
        fillObj.transform.parent = skillObj.transform;

        var fillImage = fillObj.AddComponent<Image>();
        fillImage.rectTransform.sizeDelta = new Vector2(150 ,150);

        var color = fillImage.color;
        color.a = 0.6f;
        fillImage.color = color;

        fillImage.sprite = fillSprite;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Radial360;
        fillImageList.Add(fillImage);
        strategy.OnCoolDownFillAmountValue += value => fillImage.fillAmount = value;
    }
    void OnCoolDonwFillAmount(float value) {

    }
    private void OnDestroy() {

        foreach (var obj in skillObjectsList)
            Destroy(obj.gameObject);
        skillObjectsList.Clear();
        fillImageList.Clear();
    }
}