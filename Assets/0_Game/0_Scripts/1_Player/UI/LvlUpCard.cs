using DG.Tweening;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class LvlUpCard : MonoBehaviour, IVistor {
    [SerializeField] Image back;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI textMesh;
    SkillsStrategy skillStrategy;
    LevelUpStrategy skillLvlUp;


    Button button;
    UnityEvent OnChooseCard;

    private void Awake() {

        button = GetComponent<Button>();
        button.onClick.AddListener(Choose);
    }
    public void Initialize(UnityEvent OnCardsChooseEvent) {
        OnChooseCard = OnCardsChooseEvent;
    }

    public void Rise(SkillsStrategy skillStrategy, LevelUpStrategy skillLvlUp) {
        //Animation
        transform.rotation = Quaternion.Euler(0, 90, 0);
        transform.DORotate(new Vector3(0, 0, 0), 1f, RotateMode.FastBeyond360)
               .SetEase(Ease.Linear)
               .SetUpdate(true);
        //-------------------------

        this.skillStrategy = skillStrategy;
        this.icon.sprite = skillStrategy.Icon;
        back.sprite = skillStrategy.BackSprite;

        this.skillLvlUp = skillLvlUp;
        textMesh.text = skillLvlUp.GetDescription();

    }
    [ContextMenu("Choose")]
    public void Choose() {
        skillStrategy.Accept(this);
        OnChooseCard?.Invoke();
    }
    public void Visit(object o) {
        MethodInfo visitMethod = GetType().GetMethod("Visit", new Type[] { o.GetType() });
        if (visitMethod != null && visitMethod != GetType().GetMethod("Visit", new Type[] { typeof(object) })) {
            visitMethod.Invoke(this, new object[] { o });
            Debug.Log($"Visit : {o.GetType().Name} ");
        }
    }

    public void Visit(MeleStrategy meleStrategy) {
        var skill = skillLvlUp as MeleSkillLevleUp;
        meleStrategy = skill.UpgradeSkill(meleStrategy);
        meleStrategy.UpdateValues();
    }
    public void Visit(ShieldStartegy shieldStrategy) {
        //?????
        var skill = skillLvlUp as ShieldSkillLelvelUp;
        shieldStrategy = skill.UpgradeSkill(shieldStrategy);
        shieldStrategy.UpdateValues();
    }
    public void Visit(ShootStrategy shootStrategy) {
        var skill = skillLvlUp as ShootSkillLevelUp;
        shootStrategy = skill.UpgradeSkill(shootStrategy);
        shootStrategy.UpdateValues();
    }
}
